#include "Keyboard.h"
#include "DisplayManager.h"
#include "UnityAppController.h"
#include "UnityForwardDecls.h"
#include <string>

#ifndef FILTER_EMOJIS_IOS_KEYBOARD
#define FILTER_EMOJIS_IOS_KEYBOARD 1
#endif


static KeyboardDelegate*    _keyboard = nil;

static bool                 _shouldHideInput = false;
static bool                 _shouldHideInputChanged = false;
static const unsigned       kToolBarHeight = 40;
static const unsigned       kSingleLineFontSize = 20;

extern "C" void UnityKeyboard_StatusChanged(int status);
extern "C" void UnityKeyboard_TextChanged(NSString* text);
extern "C" void UnityKeyboard_LayoutChanged(NSString* layout);

@implementation KeyboardDelegate
{
    // UI handling
    // in case of single line we use UITextField inside UIToolbar
    // in case of multi-line input we use UITextView with UIToolbar as accessory view
    // tvOS does not support multiline input thus only UITextField option is implemented
    // tvOS does not support UIToolbar so we rely on tvOS default processing
#if PLATFORM_IOS
    UITextView*     textView;

    UIToolbar*      viewToolbar;
    UIToolbar*      fieldToolbar;

    // toolbar items are kept around to prevent releasing them
    UIBarButtonItem *multiLineDone, *multiLineCancel;
    UIBarButtonItem *singleLineDone, *singleLineCancel, *singleLineInputField;

    NSLayoutConstraint* widthConstraint;
    int singleLineSystemButtonsSpace;
#endif

    UITextField*    textField;

    // inputView is view used for actual input (it will be responder): UITextField [single-line] or UITextView [multi-line]
    // editView is the "root" view for keyboard: UIToolbar [single-line] or UITextView [multi-line]
    UIView*         inputView;
    UIView*         editView;
    KeyboardShowParam cachedKeyboardParam;

    CGRect          _area;
    NSString*       initialText;

    UIKeyboardType  keyboardType;

    BOOL            _multiline;
    BOOL            _inputHidden;
    BOOL            _active;
    KeyboardStatus          _status;
    int             _characterLimit;

    // not pretty but seems like easiest way to keep "we are rotating" status
    BOOL            _rotating;
}

@synthesize area;
@synthesize active      = _active;
@synthesize status      = _status;
@synthesize text;
@synthesize selection;


- (BOOL)textFieldShouldReturn:(UITextField*)textFieldObj
{
    [self textInputDone: nil];
    return YES;
}

- (void)textInputDone:(id)sender
{
    if (_status == Visible)
    {
        _status = Done;
        UnityKeyboard_StatusChanged(_status);
    }
    [self hide];
}

- (void)textInputCancel:(id)sender
{
    _status = Canceled;
    UnityKeyboard_StatusChanged(_status);
    [self hide];
}

- (void)textInputLostFocus
{
    if (_status == Visible)
    {
        _status = LostFocus;
        UnityKeyboard_StatusChanged(_status);
    }
    [self hide];
}

- (void)textViewDidChange:(UITextView *)textView
{
    UnityKeyboard_TextChanged(textView.text);
}

- (void)textFieldDidChange:(UITextField*)textField
{
    UnityKeyboard_TextChanged(textField.text);
}

- (BOOL)textViewShouldBeginEditing:(UITextView*)view
{
#if !PLATFORM_TVOS
    view.inputAccessoryView = viewToolbar;
#endif
    return YES;
}

#if PLATFORM_IOS

- (void)keyboardWillShow:(NSNotification *)notification
{
    if (notification.userInfo == nil || inputView == nil)
        return;

    CGRect srcRect  = [[notification.userInfo objectForKey: UIKeyboardFrameEndUserInfoKey] CGRectValue];
    CGRect rect     = [UnityGetGLView() convertRect: srcRect fromView: nil];
    rect.origin.y = [UnityGetGLView() frame].size.height - rect.size.height; // iPhone X sometimes reports wrong y value for keyboard

    [self positionInput: rect x: rect.origin.x y: rect.origin.y];
}

- (void)keyboardDidShow:(NSNotification*)notification
{
    _active = YES;
    UnityKeyboard_LayoutChanged(textField.textInputMode.primaryLanguage);
}

- (void)keyboardWillHide:(NSNotification*)notification
{
    UnityKeyboard_LayoutChanged(nil);
    [self systemHideKeyboard];
}

- (void)keyboardDidChangeFrame:(NSNotification*)notification
{
    _active = true;

    CGRect srcRect  = [[notification.userInfo objectForKey: UIKeyboardFrameEndUserInfoKey] CGRectValue];
    CGRect rect     = [UnityGetGLView() convertRect: srcRect fromView: nil];

    if (rect.origin.y >= [UnityGetGLView() bounds].size.height)
        [self systemHideKeyboard];
    else
    {
        rect.origin.y = [UnityGetGLView() frame].size.height - rect.size.height; // iPhone X sometimes reports wrong y value for keyboard
        [self positionInput: rect x: rect.origin.x y: rect.origin.y];
    }
}

#endif

+ (void)Initialize
{
    NSAssert(_keyboard == nil, @"[KeyboardDelegate Initialize] called after creating keyboard");
    if (!_keyboard)
        _keyboard = [[KeyboardDelegate alloc] init];
}

+ (KeyboardDelegate*)Instance
{
    if (!_keyboard)
        _keyboard = [[KeyboardDelegate alloc] init];

    return _keyboard;
}

#if PLATFORM_IOS
- (UIToolbar*)createToolbarWithItems:(NSArray*)items
{
    UIToolbar* toolbar = [[UIToolbar alloc] initWithFrame: CGRectMake(0, 840, 320, kToolBarHeight)];
    UnitySetViewTouchProcessing(toolbar, touchesIgnored);
    toolbar.hidden = NO;
    toolbar.items = items;
    return toolbar;
}

- (void)createToolbars
{
    multiLineDone   = [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemDone target: self action: @selector(textInputDone:)];
    multiLineCancel = [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemCancel target: self action: @selector(textInputCancel:)];
    viewToolbar     = [self createToolbarWithItems: @[multiLineDone, multiLineCancel]];

    singleLineInputField = [[UIBarButtonItem alloc] initWithCustomView: textField];
    singleLineDone       = [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemDone target: self action: @selector(textInputDone:)];
    singleLineCancel     = [[UIBarButtonItem alloc] initWithBarButtonSystemItem: UIBarButtonSystemItemCancel target: self action: @selector(textInputCancel:)];
    fieldToolbar         = [self createToolbarWithItems: @[singleLineInputField, singleLineDone, singleLineCancel]];

    // Gather round boys, let's hear the story of apple ingenious api.
    // Did you see UIBarButtonItem above? oh the marvel of design
    // Maybe you thought it will have some connection to UIView or something?
    //   Yes, internally, in private members, hidden like dirty laundry in a room of a youngster
    // But, you may ask, why do we care? Oh, easy - sometimes you want to use non-english language
    // And in these languages, not good enough to be english, done/cancel items can have different sizes
    // And we insist on having input field size set because, yes, we cannot quite do a layout inside UIToolbar
    //   [because there are no views we can actually touch, thanks for asking]
    // Obviously, localizing system strings is also well hidden, and what works now might stop working tomorrow
    // That's why we keep UIBarButtonSystemItemDone/UIBarButtonSystemItemCancel above
    //   and try to translate "Done"/"Cancel" in a way that "should" work
    //   if localization fails we will still have "some" values (coming from english)
    //   and while this wont work with, say, asian languages - it should not regress the current behaviour
    UIFont* font = [UIFont systemFontOfSize: kSingleLineFontSize];
    NSBundle* uikitBundle = [NSBundle bundleForClass: UIApplication.class];
    NSString* doneStr   = [uikitBundle localizedStringForKey: @"Done" value: nil table: nil];
    NSString* cancelStr = [uikitBundle localizedStringForKey: @"Cancel" value: nil table: nil];

    // mind you, all of that is highly empirical.
    // we assume space between items to be 18 [both betwen buttons and on the sides]
    // we also assume that button width would be more less title width exactly (it should be quite close though)
    const int doneW   = [doneStr sizeWithAttributes: @{NSFontAttributeName: font}].width;
    const int cancelW = [cancelStr sizeWithAttributes: @{NSFontAttributeName: font}].width;
    singleLineSystemButtonsSpace = doneW + cancelW + 3 * 18;
}

#endif

- (id)init
{
    NSAssert(_keyboard == nil, @"You can have only one instance of KeyboardDelegate");
    self = [super init];
    if (self)
    {
#if PLATFORM_IOS
        textView = [[UITextView alloc] initWithFrame: CGRectMake(0, 840, 480, 30)];
        textView.delegate = self;
        textView.font = [UIFont systemFontOfSize: 18.0];
        textView.hidden = YES;
#endif

        textField = [[UITextField alloc] initWithFrame: CGRectMake(0, 0, 120, 30)];
        textField.delegate = self;
        textField.borderStyle = UITextBorderStyleRoundedRect;
        textField.font = [UIFont systemFontOfSize: kSingleLineFontSize];
        textField.clearButtonMode = UITextFieldViewModeWhileEditing;

#if PLATFORM_IOS
        widthConstraint = [NSLayoutConstraint constraintWithItem: textField attribute: NSLayoutAttributeWidth relatedBy: NSLayoutRelationEqual toItem: nil attribute: NSLayoutAttributeNotAnAttribute multiplier: 1.0 constant: textField.frame.size.width];
        [textField addConstraint: widthConstraint];
#endif
        [textField addTarget: self action: @selector(textFieldDidChange:) forControlEvents: UIControlEventEditingChanged];

#if PLATFORM_IOS
        [self createToolbars];
#endif

#if PLATFORM_IOS
        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(keyboardWillShow:) name: UIKeyboardWillShowNotification object: nil];
        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(keyboardDidShow:) name: UIKeyboardDidShowNotification object: nil];
        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(keyboardWillHide:) name: UIKeyboardWillHideNotification object: nil];
        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(keyboardDidChangeFrame:) name: UIKeyboardDidChangeFrameNotification object: nil];
#endif

        [[NSNotificationCenter defaultCenter] addObserver: self selector: @selector(textInputDone:) name: UITextFieldTextDidEndEditingNotification object: nil];
    }

    return self;
}

- (void)setTextInputTraits:(id<UITextInputTraits>)traits
    withParam:(KeyboardShowParam)param
    withCap:(UITextAutocapitalizationType)capitalization
{
    traits.keyboardType = param.keyboardType;
    traits.autocorrectionType = param.autocorrectionType;
    traits.keyboardAppearance = param.appearance;
    traits.autocapitalizationType = capitalization;
    // if we just switch the keyboard type for already active keyboard, the traits may not take immediate effect
    // secureTextEntry property does take effect, so toggle it to force other traits take effect too
    traits.secureTextEntry = !param.secure;
    traits.secureTextEntry = param.secure;
}

- (void)setKeyboardParams:(KeyboardShowParam)param
{
    if (!editView.hidden)
    {
        [NSObject cancelPreviousPerformRequestsWithTarget: self];
        if (cachedKeyboardParam.multiline != param.multiline ||
            cachedKeyboardParam.secure != param.secure ||
            cachedKeyboardParam.keyboardType != param.keyboardType ||
            cachedKeyboardParam.autocorrectionType != param.autocorrectionType ||
            cachedKeyboardParam.appearance != param.appearance)
        {
            [self hideUIDelayed];
        }
    }
    cachedKeyboardParam = param;

    if (_active)
        [self hide];

    initialText = param.text ? [[NSString alloc] initWithUTF8String: param.text] : @"";

    _characterLimit = param.characterLimit;

    UITextAutocapitalizationType capitalization = UITextAutocapitalizationTypeSentences;
    if (param.keyboardType == UIKeyboardTypeURL || param.keyboardType == UIKeyboardTypeEmailAddress || param.keyboardType == UIKeyboardTypeWebSearch)
        capitalization = UITextAutocapitalizationTypeNone;

#if PLATFORM_IOS
    _multiline = param.multiline;
    if (_multiline)
    {
        textView.text = initialText;
        [self setTextInputTraits: textView withParam: param withCap: capitalization];

        UITextPosition* end = [textView endOfDocument];
        UITextRange* endTextRange = [textView textRangeFromPosition: end toPosition: end];
        [textView setSelectedTextRange: endTextRange];
    }
    else
    {
        textField.text = initialText;
        [self setTextInputTraits: textField withParam: param withCap: capitalization];
        textField.placeholder = [NSString stringWithUTF8String: param.placeholder];

        UITextPosition* end = [textField endOfDocument];
        UITextRange* endTextRange = [textField textRangeFromPosition: end toPosition: end];
        [textField setSelectedTextRange: endTextRange];
    }
    inputView = _multiline ? textView : textField;
    editView = _multiline ? textView : fieldToolbar;

#else // PLATFORM_TVOS
    textField.text = initialText;
    [self setTextInputTraits: textField withParam: param withCap: capitalization];
    textField.placeholder = [NSString stringWithUTF8String: param.placeholder];
    inputView = textField;
    editView = textField;

    UITextPosition* end = [textField endOfDocument];
    UITextRange* endTextRange = [textField textRangeFromPosition: end toPosition: end];
    [textField setSelectedTextRange: endTextRange];
#endif

    [self shouldHideInput: _shouldHideInput];

    _status     = Visible;
    UnityKeyboard_StatusChanged(_status);
    _active     = YES;
}

// we need to show/hide keyboard to react to orientation too, so extract we extract UI fiddling

- (void)showUI
{
    // if we unhide everything now the input will be shown smaller then needed quickly (and resized later)
    // so unhide only when keyboard is actually shown (we will update it when reacting to ios notifications)

    [NSObject cancelPreviousPerformRequestsWithTarget: self];
    if (!inputView.isFirstResponder)
    {
        editView.hidden = YES;

        [UnityGetGLView() addSubview: editView];
        [inputView becomeFirstResponder];
    }
}

- (void)hideUI
{
    [NSObject cancelPreviousPerformRequestsWithTarget: self];
    [self performSelector: @selector(hideUIDelayed) withObject: nil afterDelay: 0.05]; // to avoid unnecessary hiding
}

- (void)hideUIDelayed
{
    [inputView resignFirstResponder];

    [editView removeFromSuperview];
    editView.hidden = YES;
}

- (void)systemHideKeyboard
{
    // when we are rotating os will bombard us with keyboardWillHide: and keyboardDidChangeFrame:
    // ignore all of them (we do it here only to simplify code: we call systemHideKeyboard only from these notification handlers)
    if (_rotating)
        return;

    _active = editView.isFirstResponder;
    editView.hidden = YES;

    _area = CGRectMake(0, 0, 0, 0);
}

- (void)show
{
    [self showUI];
}

- (void)hide
{
    [self hideUI];
}

- (void)updateInputHidden
{
    if (_shouldHideInputChanged)
    {
        [self shouldHideInput: _shouldHideInput];
        _shouldHideInputChanged = false;
    }

    textField.returnKeyType = _inputHidden ? UIReturnKeyDone : UIReturnKeyDefault;

    editView.hidden     = _inputHidden ? YES : NO;
    inputView.hidden    = _inputHidden ? YES : NO;
}

#if PLATFORM_IOS
- (void)positionInput:(CGRect)kbRect x:(float)x y:(float)y
{
    float safeAreaInsetLeft = 0;
    float safeAreaInsetRight = 0;

    if (@available(iOS 11.0, *))
    {
        safeAreaInsetLeft = [UnityGetGLView() safeAreaInsets].left;
        safeAreaInsetRight = [UnityGetGLView() safeAreaInsets].right;
    }

    if (_multiline)
    {
        // use smaller area for iphones and bigger one for ipads
        int height = UnityDeviceDPI() > 300 ? 75 : 100;

        editView.frame  = CGRectMake(safeAreaInsetLeft, y - height, kbRect.size.width - safeAreaInsetLeft - safeAreaInsetRight, height);
    }
    else
    {
        editView.frame  = CGRectMake(0, y - kToolBarHeight, kbRect.size.width, kToolBarHeight);

        // old constraint must be removed, changing value while constraint is active causes conflict when changing inputView.frame
        [inputView removeConstraint: widthConstraint];

        inputView.frame = CGRectMake(inputView.frame.origin.x,
            inputView.frame.origin.y,
            kbRect.size.width - safeAreaInsetLeft - safeAreaInsetRight - self->singleLineSystemButtonsSpace,
            inputView.frame.size.height);

        // required to avoid auto-resizing on iOS 11 in case if input text is too long
        widthConstraint.constant = inputView.frame.size.width;
        [inputView addConstraint: widthConstraint];
    }

    _area = CGRectMake(x, y, kbRect.size.width, kbRect.size.height);
    [self updateInputHidden];
}

#endif

- (CGRect)queryArea
{
    return editView.hidden ? _area : CGRectUnion(_area, editView.frame);
}

- (NSRange)querySelection
{
    UIView<UITextInput>* textInput;

#if PLATFORM_TVOS
    textInput = textField;
#else
    textInput = _multiline ? textView : textField;
#endif

    UITextPosition* beginning = textInput.beginningOfDocument;

    UITextRange* selectedRange = textInput.selectedTextRange;
    UITextPosition* selectionStart = selectedRange.start;
    UITextPosition* selectionEnd = selectedRange.end;

    const NSInteger location = [textInput offsetFromPosition: beginning toPosition: selectionStart];
    const NSInteger length = [textInput offsetFromPosition: selectionStart toPosition: selectionEnd];

    return NSMakeRange(location, length);
}

- (void)assignSelection:(NSRange)range
{
    UIView<UITextInput>* textInput;

#if PLATFORM_TVOS
    textInput = textField;
#else
    textInput = _multiline ? textView : textField;
#endif

    UITextPosition* begin = [textInput beginningOfDocument];
    UITextPosition* caret = [textInput positionFromPosition: begin offset: range.location];
    UITextPosition* select = [textInput positionFromPosition: caret offset: range.length];
    UITextRange* textRange = [textInput textRangeFromPosition: caret toPosition: select];

    [textInput setSelectedTextRange: textRange];
}

+ (void)StartReorientation
{
    if (_keyboard && _keyboard.active)
        _keyboard->_rotating = YES;
}

+ (void)FinishReorientation
{
    if (_keyboard)
        _keyboard->_rotating = NO;
}

- (NSString*)getText
{
    if (_status == Canceled)
        return initialText;
    else
    {
#if PLATFORM_TVOS
        return [textField text];
#else
        return _multiline ? [textView text] : [textField text];
#endif
    }
}

- (void)setText:(NSString*)newText
{
#if PLATFORM_IOS
    if (_multiline)
        textView.text = newText;
    else
        textField.text = newText;
#else
    textField.text = newText;
#endif
}

- (void)shouldHideInput:(BOOL)hide
{
    if (hide)
    {
        switch (keyboardType)
        {
            case UIKeyboardTypeDefault:                 hide = YES; break;
            case UIKeyboardTypeASCIICapable:            hide = YES; break;
            case UIKeyboardTypeNumbersAndPunctuation:   hide = YES; break;
            case UIKeyboardTypeURL:                     hide = YES; break;
            case UIKeyboardTypeNumberPad:               hide = NO;  break;
            case UIKeyboardTypePhonePad:                hide = NO;  break;
            case UIKeyboardTypeNamePhonePad:            hide = NO;  break;
            case UIKeyboardTypeEmailAddress:            hide = YES; break;
            case UIKeyboardTypeTwitter:                 hide = YES; break;
            case UIKeyboardTypeWebSearch:               hide = YES; break;
            case UIKeyboardTypeDecimalPad:              hide = NO; break;
            default:                                    hide = NO;  break;
        }
    }

    _inputHidden = hide;
}

#if FILTER_EMOJIS_IOS_KEYBOARD

static bool StringContainsEmoji(NSString *string);
- (BOOL)textField:(UITextField*)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString*)string_
{
    if (range.length + range.location > textField.text.length)
        return NO;

    return [self currentText: textField.text shouldChangeInRange: range replacementText: string_] && !StringContainsEmoji(string_);
}

- (BOOL)textView:(UITextView*)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString*)text_
{
    if (range.length + range.location > textView.text.length)
        return NO;

    return [self currentText: textView.text shouldChangeInRange: range replacementText: text_] && !StringContainsEmoji(text_);
}

#else

- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString*)string_
{
    if (range.length + range.location > textField.text.length)
        return NO;

    return [self currentText: textField.text shouldChangeInRange: range replacementText: string_];
}

- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString*)text_
{
    if (range.length + range.location > textView.text.length)
        return NO;

    return [self currentText: textView.text shouldChangeInRange: range replacementText: text_];
}

#endif // FILTER_EMOJIS_IOS_KEYBOARD

- (BOOL)currentText:(NSString*)currentText shouldChangeInRange:(NSRange)range  replacementText:(NSString*)text_
{
    NSUInteger newLength = currentText.length + (text_.length - range.length);
    if (newLength > _characterLimit && _characterLimit != 0 && newLength >= currentText.length)
    {
        NSString* newReplacementText = @"";
        if ((currentText.length - range.length) < _characterLimit)
            newReplacementText = [text_ substringWithRange: NSMakeRange(0, _characterLimit - (currentText.length - range.length))];

        NSString* newText = [currentText stringByReplacingCharactersInRange: range withString: newReplacementText];

#if PLATFORM_IOS
        if (_multiline)
            [textView setText: newText];
        else
            [textField setText: newText];
#else
        [textField setText: newText];
#endif

        return NO;
    }
    else
    {
        return YES;
    }
}

@end

//==============================================================================
//
//  Unity Interface:

extern "C" void UnityKeyboard_Create(unsigned keyboardType, int autocorrection, int multiline, int secure, int alert, const char* text, const char* placeholder, int characterLimit)
{
#if PLATFORM_TVOS
    // Not supported. The API for showing keyboard for editing multi-line text
    // is not available on tvOS
    multiline = false;
#endif

    static const UIKeyboardType keyboardTypes[] =
    {
        UIKeyboardTypeDefault,
        UIKeyboardTypeASCIICapable,
        UIKeyboardTypeNumbersAndPunctuation,
        UIKeyboardTypeURL,
        UIKeyboardTypeNumberPad,
        UIKeyboardTypePhonePad,
        UIKeyboardTypeNamePhonePad,
        UIKeyboardTypeEmailAddress,
        UIKeyboardTypeDefault, // Default is used in case Wii U specific NintendoNetworkAccount type is selected (indexed at 8 in UnityEngine.TouchScreenKeyboardType)
        UIKeyboardTypeTwitter,
        UIKeyboardTypeWebSearch,
        UIKeyboardTypeDecimalPad
    };

    static const UITextAutocorrectionType autocorrectionTypes[] =
    {
        UITextAutocorrectionTypeNo,
        UITextAutocorrectionTypeDefault,
    };

    static const UIKeyboardAppearance keyboardAppearances[] =
    {
        UIKeyboardAppearanceDefault,
        UIKeyboardAppearanceAlert,
    };

    KeyboardShowParam param =
    {
        text, placeholder,
        keyboardTypes[keyboardType],
        autocorrectionTypes[autocorrection],
        keyboardAppearances[alert],
        (BOOL)multiline, (BOOL)secure,
        characterLimit
    };

    [[KeyboardDelegate Instance] setKeyboardParams: param];
}

extern "C" void UnityKeyboard_Show()
{
    // do not send hide if didnt create keyboard
    // TODO: probably assert?
    if (!_keyboard)
        return;

    [[KeyboardDelegate Instance] show];
}

extern "C" void UnityKeyboard_Hide()
{
    // do not send hide if didnt create keyboard
    // TODO: probably assert?
    if (!_keyboard)
        return;

    [[KeyboardDelegate Instance] textInputLostFocus];
}

extern "C" void UnityKeyboard_SetText(const char* text)
{
    [KeyboardDelegate Instance].text = [NSString stringWithUTF8String: text];
}

extern "C" NSString* UnityKeyboard_GetText()
{
    return [KeyboardDelegate Instance].text;
}

extern "C" int UnityKeyboard_IsActive()
{
    return (_keyboard && _keyboard.active) ? 1 : 0;
}

extern "C" int UnityKeyboard_Status()
{
    return _keyboard ? _keyboard.status : Canceled;
}

extern "C" void UnityKeyboard_SetInputHidden(int hidden)
{
    _shouldHideInput        = hidden;
    _shouldHideInputChanged = true;

    // update hidden status only if keyboard is on screen to avoid showing input view out of nowhere
    if (_keyboard && _keyboard.active)
        [_keyboard updateInputHidden];
}

extern "C" int UnityKeyboard_IsInputHidden()
{
    return _shouldHideInput ? 1 : 0;
}

extern "C" void UnityKeyboard_GetRect(float* x, float* y, float* w, float* h)
{
    CGRect area = _keyboard ? _keyboard.area : CGRectMake(0, 0, 0, 0);

    // convert to unity coord system

    float   multX   = (float)GetMainDisplaySurface()->targetW / UnityGetGLView().bounds.size.width;
    float   multY   = (float)GetMainDisplaySurface()->targetH / UnityGetGLView().bounds.size.height;

    *x = 0;
    *y = area.origin.y * multY;
    *w = area.size.width * multX;
    *h = area.size.height * multY;
}

extern "C" void UnityKeyboard_SetCharacterLimit(unsigned characterLimit)
{
    [KeyboardDelegate Instance].characterLimit = characterLimit;
}

extern "C" int UnityKeyboard_CanGetSelection()
{
    return (_keyboard) ? 1 : 0;
}

extern "C" void UnityKeyboard_GetSelection(int* location, int* length)
{
    if (_keyboard)
    {
        NSRange selection = _keyboard.selection;

        *location = (int)selection.location;
        *length = (int)selection.length;
    }
    else
    {
        *location = 0;
        *length = 0;
    }
}

extern "C" int UnityKeyboard_CanSetSelection()
{
    return (_keyboard) ? 1 : 0;
}

extern "C" void UnityKeyboard_SetSelection(int location, int length)
{
    if (_keyboard)
    {
        NSRange range = NSMakeRange(location, length);
        _keyboard.selection = range;
    }
}

//==============================================================================
//
//  Emoji Filtering: unicode magic

#if FILTER_EMOJIS_IOS_KEYBOARD
static bool StringContainsEmoji(NSString *string)
{
    __block BOOL returnValue = NO;

    [string enumerateSubstringsInRange: NSMakeRange(0, string.length)
 options: NSStringEnumerationByComposedCharacterSequences
 usingBlock:^(NSString* substring, NSRange substringRange, NSRange enclosingRange, BOOL* stop)
    {
        const unichar hs = [substring characterAtIndex: 0];
        const unichar ls = substring.length > 1 ? [substring characterAtIndex: 1] : 0;

            #define IS_IN(val, min, max) (((val) >= (min)) && ((val) <= (max)))

        if (IS_IN(hs, 0xD800, 0xDBFF))
        {
            if (substring.length > 1)
            {
                const int uc = ((hs - 0xD800) * 0x400) + (ls - 0xDC00) + 0x10000;

                // Musical: [U+1D000, U+1D24F]
                // Enclosed Alphanumeric Supplement: [U+1F100, U+1F1FF]
                // Enclosed Ideographic Supplement: [U+1F200, U+1F2FF]
                // Miscellaneous Symbols and Pictographs: [U+1F300, U+1F5FF]
                // Supplemental Symbols and Pictographs: [U+1F900, U+1F9FF]
                // Emoticons: [U+1F600, U+1F64F]
                // Transport and Map Symbols: [U+1F680, U+1F6FF]
                if (IS_IN(uc, 0x1D000, 0x1F9FF))
                    returnValue = YES;
            }
        }
        else if (substring.length > 1 && ls == 0x20E3)
        {
            // emojis for numbers: number + modifier ls = U+20E3
            returnValue = YES;
        }
        else
        {
            if (        // Latin-1 Supplement
                hs == 0x00A9 || hs == 0x00AE
                // General Punctuation
                ||  hs == 0x203C || hs == 0x2049
                // Letterlike Symbols
                ||  hs == 0x2122 || hs == 0x2139
                // Arrows
                ||  IS_IN(hs, 0x2194, 0x2199) || IS_IN(hs, 0x21A9, 0x21AA)
                // Miscellaneous Technical
                ||  IS_IN(hs, 0x231A, 0x231B) || IS_IN(hs, 0x23E9, 0x23F3) || IS_IN(hs, 0x23F8, 0x23FA) || hs == 0x2328 || hs == 0x23CF
                // Geometric Shapes
                ||  IS_IN(hs, 0x25AA, 0x25AB) || IS_IN(hs, 0x25FB, 0x25FE) || hs == 0x25B6 || hs == 0x25C0
                // Miscellaneous Symbols
                ||  IS_IN(hs, 0x2600, 0x2604) || IS_IN(hs, 0x2614, 0x2615) || IS_IN(hs, 0x2622, 0x2623) || IS_IN(hs, 0x262E, 0x262F)
                ||  IS_IN(hs, 0x2638, 0x263A) || IS_IN(hs, 0x2648, 0x2653) || IS_IN(hs, 0x2665, 0x2666) || IS_IN(hs, 0x2692, 0x2694)
                ||  IS_IN(hs, 0x2696, 0x2697) || IS_IN(hs, 0x269B, 0x269C) || IS_IN(hs, 0x26A0, 0x26A1) || IS_IN(hs, 0x26AA, 0x26AB)
                ||  IS_IN(hs, 0x26B0, 0x26B1) || IS_IN(hs, 0x26BD, 0x26BE) || IS_IN(hs, 0x26C4, 0x26C5) || IS_IN(hs, 0x26CE, 0x26CF)
                ||  IS_IN(hs, 0x26D3, 0x26D4) || IS_IN(hs, 0x26D3, 0x26D4) || IS_IN(hs, 0x26E9, 0x26EA) || IS_IN(hs, 0x26F0, 0x26F5)
                ||  IS_IN(hs, 0x26F7, 0x26FA)
                ||  hs == 0x260E || hs == 0x2611 || hs == 0x2618 || hs == 0x261D || hs == 0x2620 || hs == 0x2626 || hs == 0x262A
                ||  hs == 0x2660 || hs == 0x2663 || hs == 0x2668 || hs == 0x267B || hs == 0x267F || hs == 0x2699 || hs == 0x26C8
                ||  hs == 0x26D1 || hs == 0x26FD
                // Dingbats
                ||  IS_IN(hs, 0x2708, 0x270D) || IS_IN(hs, 0x2733, 0x2734) || IS_IN(hs, 0x2753, 0x2755)
                ||  IS_IN(hs, 0x2763, 0x2764) || IS_IN(hs, 0x2795, 0x2797)
                ||  hs == 0x2702 || hs == 0x2705 || hs == 0x270F || hs == 0x2712 || hs == 0x2714 || hs == 0x2716 || hs == 0x271D
                ||  hs == 0x2721 || hs == 0x2728 || hs == 0x2744 || hs == 0x2747 || hs == 0x274C || hs == 0x274E || hs == 0x2757
                ||  hs == 0x27A1 || hs == 0x27B0 || hs == 0x27BF
                // CJK Symbols and Punctuation
                ||  hs == 0x3030 || hs == 0x303D
                // Enclosed CJK Letters and Months
                ||  hs == 0x3297 || hs == 0x3299
                // Supplemental Arrows-B
                ||  IS_IN(hs, 0x2934, 0x2935)
                // Miscellaneous Symbols and Arrows
                ||  IS_IN(hs, 0x2B05, 0x2B07) || IS_IN(hs, 0x2B1B, 0x2B1C) || hs == 0x2B50 || hs == 0x2B55
            )
            {
                returnValue = YES;
            }
        }

            #undef IS_IN
    }];

    return returnValue;
}

#endif // FILTER_EMOJIS_IOS_KEYBOARD

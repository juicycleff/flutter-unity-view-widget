#pragma once

typedef struct
{
    const char* text;
    const char* placeholder;

    UIKeyboardType              keyboardType;
    UITextAutocorrectionType    autocorrectionType;
    UIKeyboardAppearance        appearance;

    BOOL multiline;
    BOOL secure;
    int characterLimit;
}
KeyboardShowParam;


@interface KeyboardDelegate : NSObject<UITextFieldDelegate, UITextViewDelegate>
{
}
- (BOOL)textFieldShouldReturn:(UITextField*)textField;
- (void)textInputDone:(id)sender;
- (void)textInputCancel:(id)sender;
- (void)textInputLostFocus;
- (void)textViewDidChange:(UITextView *)textView;
- (void)keyboardWillShow:(NSNotification*)notification;
- (void)keyboardDidShow:(NSNotification*)notification;
- (void)keyboardWillHide:(NSNotification*)notification;

// on older devices initial keyboard creation might be slow, so it is good to init in on initial loading.
// on the other hand, if you dont use keyboard (or use it rarely), you can avoid having all related stuff in memory:
//     keyboard will be created on demand anyway (in Instance method)
+ (void)Initialize;
+ (KeyboardDelegate*)Instance;

- (id)init;
- (void)setKeyboardParams:(KeyboardShowParam)param;
- (void)show;
- (void)hide;
- (void)positionInput:(CGRect)keyboardRect x:(float)x y:(float)y;
- (void)shouldHideInput:(BOOL)hide;

+ (void)StartReorientation;
+ (void)FinishReorientation;

- (CGRect)queryArea;
- (NSString*)getText;
- (void)setText:(NSString*)newText;

@property (readonly, nonatomic, getter = queryArea)               CGRect          area;
@property (readonly, nonatomic)                                 BOOL            active;
@property (readonly, nonatomic)                                 KeyboardStatus  status;
@property (retain, nonatomic, getter = getText, setter = setText:)  NSString*       text;
@property (assign, nonatomic)   int characterLimit;
@property (readonly, nonatomic)                                 BOOL        canGetSelection;
@property (nonatomic, getter = querySelection, setter = assignSelection:)  NSRange   selection;

@end

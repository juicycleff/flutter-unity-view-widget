image: node:6.10.0

stages:
  - push_to_packman_staging

push_to_packman_staging:
  stage: push_to_packman_staging
  only:
    - tags
  script:
    - sed -i "s/0.0.1-PLACEHOLDERVERSION/$CI_COMMIT_TAG/g" package.json
    - sed -i "s/PLACEHOLDERSHA/$CI_COMMIT_SHA/g" package.json
    - sed -i "s/0.0.1-PLACEHOLDERVERSION/$CI_COMMIT_TAG/g" CHANGELOG.md
    - curl -u $USER_NAME:$API_KEY https://staging-packages.unity.com/auth > .npmrc
    - npm publish

#!/usr/bin/env bash

if [ -z "$IOS_ANALYTICSKEY" ]
then
    echo "You need define the IOS_ANALYTICSKEY environment variable"
    exit 1
fi

if [ -z "$ANDROID_ANALYTICSKEY" ]
then
    echo "You need define the ANDROID_ANALYTICSKEY environment variable"
    exit 1
fi

if [ -z "$VERSION_NAME" ]
then
    echo "You need define the VERSION_NAME environment variable"
    exit 1
fi

VERSION_STRING = $VERSION_NAME

if [ ! -z "$PREVIEW_BUILD" ]
then
    echo "Setting Build to preview version"

    if [ -z "$APPCENTER_BUILD_ID" ]
    then
        echo "APPCENTER_BUILD_ID environment variable not found"
        exit 1
    fi

    VERSION_STRING="$VERSION_NAME-preview-$APPCENTER_BUILD_ID"

fi

#Set the IOS app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs IosAnalyticsKey $IOS_ANALYTICSKEY

#Set the Android app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs AndroidAnalyticsKey $ANDROID_ANALYTICSKEY

#Set the Android version name
./set-android-build-version.sh ../src/Osma.Mobile.App.Android/Properties/AndroidManifest.xml $VERSION_STRING

#Set the iOS version name
./set-ios-build-version.sh ../src/Osma.Mobile.App.iOS/Info.plist $VERSION_STRING

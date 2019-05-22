#!/usr/bin/env bash
if [ "$#" -ne 2 ] 
then
    echo "Illegal number of parameters please specify ANDROID_MANIFEST_FILE_PATH VERSION_NAME in the order shown"
    exit
fi

ANDROID_MANIFEST_FILE_PATH=$1
VERSION_NAME=$2

echo "Updating version name to $VERSION_NAME in $ANDROID_MANIFEST_FILE_PATH"
sed -i 's/versionName="[0-9.]*"/versionName="'$VERSION_NAME'"/' $ANDROID_MANIFEST_FILE_PATH

echo "File content:"
cat $ANDROID_MANIFEST_FILE_PATH
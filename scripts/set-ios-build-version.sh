#!/usr/bin/env bash
if [ "$#" -ne 2 ] 
then
    echo "Illegal number of parameters please specify INFO_PLIST_FILE VERSION_NAME in the order shown"
    exit
fi

INFO_PLIST_FILE=$1
VERSION_NAME=$2

echo "Updating version name to $VERSION_NAME in $INFO_PLIST_FILE"
plutil -replace CFBundleShortVersionString -string $VERSION_NAME $INFO_PLIST_FILE

echo "File content:"
cat $INFO_PLIST_FILE
var MobileDetect = {
    IsMobile: function() {
        return ysdk.deviceInfo.type != "desktop"
    }
}

mergeInto(LibraryManager.library, MobileDetect);
﻿function ScrollViewer() {
    this.initialize.apply(this, arguments);
}
ScrollViewer.prototype = {
    initialize: function (container) {
        //
        // Dragging Cursor Set.
        //
        // http://docs.jquery.com/Utilities/jQuery.browser
        // jQuery.browser Deprecated in jQuery 1.3
        //
        //if (navigator.userAgent.indexOf("Gecko/") != -1) {
        var browserType = Common.GetBrowserType();
        if (Common.BROWSER_CHROME == browserType) {
            //this.grab = "-moz-grab";
            //this.grabbing = "-moz-grabbing";
            this.grab = "-webkit-grab";
            this.grabbing = "-webkit-grabbing";
        } else {
            //this.grab = "default";
            //this.grabbing = "move";
            this.grab = "move";
            this.grabbing = "all-scroll";
        }

        // Get container and image.
        this.m = $(container);
        this.i = this.m.children().css("cursor", this.grab);

        this.isgrabbing = false;

        // Set mouse events.
        var self = this;
        this.i.mousedown(function (e) {
            self.startgrab();
            this.xp = e.pageX;
            this.yp = e.pageY;
            return false;
        }).mousemove(function (e) {
            if (!self.isgrabbing) return true;
            self.scrollTo(this.xp - e.pageX, this.yp - e.pageY);
            this.xp = e.pageX;
            this.yp = e.pageY;
            return false;
        })
        .mouseout(function () { self.stopgrab() })
        .mouseup(function () { self.stopgrab() })
        .dblclick(function () {
            var _m = self.m;
            var off = _m.offset();
            var dx = this.xp - off.left - _m.width() / 2;
            if (dx < 0) {
                dx = "+=" + dx + "px";
            } else {
                dx = "-=" + -dx + "px";
            }
            var dy = this.yp - off.top - _m.height() / 2;
            if (dy < 0) {
                dy = "+=" + dy + "px";
            } else {
                dy = "-=" + -dy + "px";
            }
            _m.animate({ scrollLeft: dx, scrollTop: dy },
                    "normal", "swing");
        });

        this.centering();
    },
    centering: function () {
        var _m = this.m;
        var w = this.i.width() - _m.width();
        var h = this.i.height() - _m.height();
        _m.scrollLeft(w / 2).scrollTop(h / 2);
    },
    startgrab: function () {
        this.isgrabbing = true;
        this.i.css("cursor", this.grabbing);
    },
    stopgrab: function () {
        this.isgrabbing = false;
        this.i.css("cursor", this.grab);
    },
    scrollTo: function (dx, dy) {
        var _m = this.m;
        var x = _m.scrollLeft() + dx;
        var y = _m.scrollTop() + dy;
        _m.scrollLeft(x).scrollTop(y);
    }
}
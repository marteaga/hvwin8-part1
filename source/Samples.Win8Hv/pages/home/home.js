(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/home/home.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var self = this;
            // attempt to make the request
            Application.loginManager.ping(function () {
                // we are good
                console.log("already logged in and cookie set");
            },
            function (result) {
                console.log("need to sign in");
                self.showSettings();
            });
        },

        // show settings to try and sign in
        showSettings: function () {
            // wire up the completed events
            Application.loginManager.onLoginComplete = function (result) {
                // we are good so attempt to get data
                console.log('sign in successful');
            };

            // wire up the failed event
            Application.loginManager.onLoginFailed = function (result) {
                // show a message to user
                console.log('login failed!!');
            };

            // since we can't sign in show the login
            WinJS.UI.SettingsFlyout.showSettings("login", "/pages/login/login.html");
        }
    });
})();

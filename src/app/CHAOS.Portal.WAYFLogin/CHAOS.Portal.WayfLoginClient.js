var CHAOS;
(function (CHAOS) {
    /// <reference path="TypeScriptDefinitions/PortalClient.d.ts" />
    (function (Portal) {
        var WayfLoginClient = (function () {
            function WayfLoginClient() {
            }
            WayfLoginClient.Login = function (client, wayfServicePath, frame, callback) {
                if (!client.SessionAcquired())
                    throw "IPortalClient does not have a session";

                if (wayfServicePath.substr(wayfServicePath.length - 1, 1) != "/")
                    wayfServicePath += "/";

                if (callback != null) {
                    var messageRecieved = function (event) {
                        window.removeEventListener("message", messageRecieved, false);
                        callback(event.data == "success");
                    };

                    window.addEventListener("message", messageRecieved, false);
                }

                frame.src = wayfServicePath + "CHAOSWayfLogin.php?sessionGuid=" + client.GetCurrentSession().Guid + "&apiPath=" + client.GetServicePath();
            };
            return WayfLoginClient;
        })();
        Portal.WayfLoginClient = WayfLoginClient;
    })(CHAOS.Portal || (CHAOS.Portal = {}));
    var Portal = CHAOS.Portal;
})(CHAOS || (CHAOS = {}));
//# sourceMappingURL=CHAOS.Portal.WayfLoginClient.js.map

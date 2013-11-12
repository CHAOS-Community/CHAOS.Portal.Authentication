/// <reference path="TypeScriptDefinitions/PortalClient.d.ts" />
var CHAOS;
(function (CHAOS) {
    var WayfLoginClient = (function () {
        function WayfLoginClient() {
        }
        WayfLoginClient.Login = function (client, wayfServicePath, frame, callback) {
            if (!client.SessionAcquired())
                throw "IPortalClient does not have a session";

            frame.onload = function (e) {
                var statusIndex = frame.contentWindow.location.hash.indexOf("status");

                if (statusIndex == -1)
                    return;

                callback(frame.contentWindow.location.hash.substr(statusIndex + 7, 7) == "success");
            };

            frame.src = wayfServicePath + "CHAOSWayfLogin.php?sessionGuid=" + client.GetCurrentSession().Guid + "&apiPath=" + client.GetServicePath();
        };
        return WayfLoginClient;
    })();
    CHAOS.WayfLoginClient = WayfLoginClient;
})(CHAOS || (CHAOS = {}));
//# sourceMappingURL=CHAOS.Portal.WayfLoginClient.js.map

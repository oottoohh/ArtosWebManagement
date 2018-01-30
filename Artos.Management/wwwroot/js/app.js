/// <reference path="oidc-client.js" />

function log() {


    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        
    });
}



var config = {
    authority: "http://localhost:5000",
    client_id: "webapp2",
    redirect_uri: "http://localhost:5005/signin-oidc",
    response_type: "id_token token",
    scope:"openid profile masterdataapi",
    post_logout_redirect_uri: "http://localhost:5005/signout-callback-oidc",

};



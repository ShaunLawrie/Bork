// submit bork on click and load it into the feed
$("#new-bork-form").submit(function(ev) {
    var serializedObject = $("#new-bork-form").serializeObject();
    console.log("Bork should be submitted:");
    console.log(serializedObject);
    $.ajax({
        type: "POST",
        url: "/bork",
        // The key needs to match your method's input parameter (case-sensitive).
        data: JSON.stringify(serializedObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log("pass");
            console.log(data);
            $("#content").val("");
            reloadContainer("#profile-stats", "/account/stats");
            reloadContainer("#top-borks-container", "/bork/topborks");
        },
        failure: function (errMsg) {
            console.log("fail");
            console.log(data);
        }
    });
    ev.preventDefault();
    return false;
});

// rebork an existing bork on click and load it into the feed
function rebork (el, ev) {
    var id = $(el).attr("id");
    id = id.replace("bork-", "");
    console.log("ReBork should be submitted for original id " + id);
    $.ajax({
        type: "POST",
        url: "/rebork/" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log("pass");
            console.log(data);
            reloadContainer("#profile-stats", "/account/stats");
            reloadContainer("#top-borks-container", "/bork/topborks");
        },
        failure: function (errMsg) {
            console.log("fail");
            console.log(data);
        }
    });
    ev.preventDefault();
}

// reload stats for user
setInterval(function () {
    console.log("Reloading stats");
    reloadContainer("#profile-stats", "/account/stats");
}, 5000);

// reload feed
setInterval(function () {
    console.log("Reloading feed");
    reloadContainer("#top-borks-container", "/bork/topborks");
}, 5000);

function reloadContainer(containerId, url) {
    $(containerId).load(url);
}
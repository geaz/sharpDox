$(document).ready(function () {
	if (window.attachEvent) { // ::sigh:: IE8 support
	   window.attachEvent("onmessage", handleMessage, false);
	} else {
		window.addEventListener("message", handleMessage, false);
	}
});

function handleMessage(e) {
    if (e.data.action == 'RESIZE') {
        var targetHeight = e.data.height;
        $('#docFrame').height(targetHeight);
    }
}

var activeDiv;

$(window).on("load", function () {
    $(window).hashchange(function() {
        onHashChange();
    });

	$(window).hashchange();
});

function onHashChange(){
	if(window.location.hash) {
		if(activeDiv != undefined) activeDiv.hide();
		activeDiv = $(window.location.hash);

		$(window.location.hash).show();
		resize();
	} else {
		if(activeDiv != undefined) activeDiv.hide();
		activeDiv = $("#typeIndex");

		$("#typeIndex").show();
		resize();
	}
}

function resize(){
    var body = document.body,
    html = document.documentElement,
    height = body.offsetHeight + 75;
    if(height === 0){
        height = html.offsetHeight + 75;
    }
    parent.postMessage({'action':'RESIZE', 'height':height}, '*');
}
$(window).on("load", function () {
    resize();
});

function resize(){
    var body = document.body,
    html = document.documentElement,
    height = body.offsetHeight + 50;
    if(height === 0){
        height = html.offsetHeight + 50;
    }
    parent.postMessage({'action':'RESIZE', 'height':height}, '*');
}
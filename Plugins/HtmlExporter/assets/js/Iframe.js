$(document).ready(function () {
    resize();
});

function resize(){
    var body = document.body,
    html = document.documentElement,
    height = body.offsetHeight + 100;
    if(height === 0){
        height = html.offsetHeight + 100;
    }
    parent.postMessage({'action':'RESIZE', 'height':height}, '*');
}
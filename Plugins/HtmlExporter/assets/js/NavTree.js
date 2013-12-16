var urlPrefix;

$(document).ready(function () {
    urlPrefix = "../";
    if ($('meta[name=indexpage]').length > 0) urlPrefix = "";

    initNav();
});

function initNav() {
    var nav = $("#navigation-container");
    nav.load(urlPrefix + "nav/" + nav.attr("data-nav") + ".html", function () {
        initNavOnClick();
        initNavUrls();
        initImgUrls();
        initNavWrap();
    });
}

function initNavUrls() {
    var links = $("a").not('[href^="http://"],[href="#"],[href^="../"]');
    $.each(links, function (key, value) {
        if ($(value).is('[href]')) {
            $(value).attr('href', urlPrefix + $(value).attr('href'));
        }
        else {
            $(value).attr('xlink:href', urlPrefix + $(value).attr('xlink:href'));
        }
    });
}

function initImgUrls() {
    var images = $("img").not('[src^="http://"],[src^="../"]');
    $.each(images, function (key, value) {
        var src = $(value).attr('src');
        $(value).attr('src', urlPrefix + src);
    });
}

function initNavOnClick() {
    var items = $("#navigation a[href='#']");
    items.click(function () {
        if ($(this).is("[data-nav]")) {
            Go(this);
        }
    });
}

function Go(item) {
    var nav = $("#navigation-container");
    nav.load(urlPrefix + "nav/" + $(item).attr("data-nav") + ".html", function () {
        initNavOnClick();
        initNavUrls();
        initImgUrls();
        initNavWrap();
    });
}

function initNavWrap() {
    var items = $("#sidebar #navigation li a p");
    $.each(items, function (key, val) {
        var lineBreaks = $(val).width() / 240;
        if (lineBreaks > 1) {
            var parent = $(val).parent();
            var splittedText = $(val).html().split(".");
            var textIndex = 0;
            $(val).remove();
            for (var i = 0; i < lineBreaks; i++) {
                textIndex = createNewParagraph(parent, splittedText, textIndex);
            }
        }
    });
}

function createNewParagraph(parent, splittedText, textIndex) {
    var newP = $("<p>" + joinFromTillIndex(splittedText, textIndex, splittedText.length - 1) + "</p>");
    var newIndex = splittedText.length - 1;
    parent.append(newP);
    for (var i = splittedText.length - 1; i >= textIndex; i--) {
        if (newP.width() > 240) {
            newP.html(joinFromTillIndex(splittedText, textIndex, i));

            if (i + 1 != splittedText.length) {
                newP.html(newP.html() + ".");
            }

            newIndex = i;
        }
        else {
            break;
        }
    }
    newIndex++;
    return newIndex;
}

function joinFromTillIndex(array, from, till) {
    var text = "";
    for (var i = from; i <= till; i++) {
        text += array[i] + ".";
    }
    text = text.substring(0, text.length - 1);
    return text;
}
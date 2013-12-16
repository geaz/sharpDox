$(document).ready(function(){
	$('#sd-sequence-diagram svg').panzoom();
	
	$('#sd-sequence-diagram svg').parent().on('mousewheel.focal', function( e ) {
		e.preventDefault();
		var delta = e.delta || e.originalEvent.wheelDelta;
		var zoomOut = delta ? delta < 0 : e.originalEvent.deltaY > 0;
		$('#sd-sequence-diagram svg').panzoom('zoom', zoomOut, {
			increment: 0.1,
			focal: e
		});
	});
});
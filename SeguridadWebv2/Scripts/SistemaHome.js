(function ($) {
    "use strict"; // Start of use strict

    // jQuery for page scrolling feature - requires jQuery Easing plugin
    $('a.page-scroll').bind('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($anchor.attr('href')).offset().top - 50)
        }, 1250, 'easeInOutExpo');
        event.preventDefault();
    });

    // Highlight the top nav as scrolling occurs
    $('body').scrollspy({
        target: '.navbar-fixed-top',
        offset: 51
    });

    // Closes the Responsive Menu on Menu Item Click
    $('.navbar-collapse ul li a').click(function () {
        $('.navbar-toggle:visible').click();
    });

    // Offset for Main Navigation
    $('#mainNav').affix({
        offset: {
            top: 100
        }
    })

    // Initialize and Configure Scroll Reveal Animation
    window.sr = ScrollReveal();
    sr.reveal('.sr-icons', {
        duration: 600,
        scale: 0.3,
        distance: '0px'
    }, 200);
    sr.reveal('.sr-button', {
        duration: 1000,
        delay: 200
    });
    sr.reveal('.sr-contact', {
        duration: 600,
        scale: 0.3,
        distance: '0px'
    }, 300);

    // Initialize and Configure Magnific Popup Lightbox Plugin
    $('.popup-gallery').magnificPopup({
        delegate: 'a',
        type: 'image',
        tLoading: 'Loading image #%curr%...',
        mainClass: 'mfp-img-mobile',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">The image #%curr%</a> could not be loaded.'
        }
    });


    var searchBoxBloodHound = new Bloodhound({
        datumTokenizer: function (datum) {
            return Bloodhound.tokenizers.whitespace(datum.NombreEspecialidad);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Home/BuscarEspecialidades?busqueda=%QUERY',
            wildcard: '%QUERY',
            // the returned json contains an array of strings, but the Bloodhound
            // suggestion engine expects JavaScript objects so this converts all of
            // those strings into objects containing the properties, title and author
            filter: function (resultados) {
                return $.map(resultados, function (data) {
                    return {
                        especialidad: data
                    }
                });
            },

        },

    });

    searchBoxBloodHound.initialize();

    $('#scrollable-dropdown-menu #mensaje-busqueda #searchBox').typeahead(null, {
        displayKey: 'especialidad',
        source: searchBoxBloodHound.ttAdapter(),
        limit: 10,
        templates: {
            empty: [
              '<div class="empty-message">',
                'Lo siento, no se pudo encontrar la especialidad y/o el profesional.',
              '</div>'
            ].join('\n'),
            suggestion: Handlebars.compile('<p><strong>{{especialidad}}</strong></p>')
        }
    });


})(jQuery); // End of use strict

//<!---------------------------     GTM     ---------------------------!>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-XXXXXXX');

//<!--------------------     GA link tracking     -------------------!>
var filetypes = /\.(zip|exe|dmg|pdf|doc.*|xls.*|ppt.*|mp3|txt|rar|wma|mov|avi|wmv|flv|wav)$/i;
    var baseHref = '';
    if ($('base').attr('href') != undefined) baseHref = jQuery('base').attr('href');

    $('a').on('click', function (event) {
        var el = jQuery(this);
        var track = true;
        var href = (typeof (el.attr('href')) != 'undefined') ? el.attr('href') : "";
        var isThisDomain = href.match(document.domain.split('.').reverse()[1] + '.' + document.domain.split('.').reverse()[0]);
        if (!href.match(/^javascript:/i)) {
            var elEv = []; elEv.value = 0, elEv.non_i = false;
            if (href.match(/^mailto\:/i)) {
                elEv.category = "email";
                elEv.action = "click";
                elEv.label = href.replace(/^mailto\:/i, '');
                elEv.loc = href;
            }
            else if (href.match(filetypes)) {
                var extension = (/[.]/.exec(href)) ? /[^.]+$/.exec(href) : undefined;
                elEv.category = "download";
                elEv.action = "click-" + extension[0];
                elEv.label = href.replace(/ /g, "-");
                elEv.loc = baseHref + href;
            }
            else if (href.match(/^https?\:/i) && !isThisDomain) {
                elEv.category = "outbound-link";
                elEv.action = "click";
                elEv.label = href.replace(/^https?\:\/\//i, '');
                elEv.non_i = true;
                elEv.loc = href;
            }
            else if (href.match(/^tel\:/i)) {
                elEv.category = "telephone";
                elEv.action = "click";
                elEv.label = href.replace(/^tel\:/i, '');
                elEv.loc = href;
            }
            else track = false;

            if (track) {
                                                                
                                                                gtag('event', elEv.action.toLowerCase(), {
                                                                                'event_category': elEv.category.toLowerCase(),
                                                                                'event_label': elEv.label.toLowerCase(),
                                                                                'transport_type': 'beacon'
                                                                  });
                                                                //,'event_callback': function(){document.location = elEv.loc;}
               //_gaq.push(['_trackEvent', elEv.category.toLowerCase(), elEv.action.toLowerCase(), elEv.label.toLowerCase(), elEv.value, elEv.non_i]);
                if (el.attr('target') == undefined || el.attr('target').toLowerCase() != '_blank') {
                    setTimeout(function(){document.location = elEv.loc; }, 400);
                    return false;
                }
            }
        }
    });

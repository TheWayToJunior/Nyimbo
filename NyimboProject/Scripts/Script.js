var Obj, Page, Song = new Audio();

$(document).ready(function () {
    $.ajax({
        beforeSend: function (data) {

            $('#form0').submit(function () {
                if (!$('#TextBox1').val() || $('#TextBox1').val() == '') {
                    $($('#TextBox1')).css('border-color', 'red');

                    return false;
                }
            });
        },
    });

    $('#TextBox1').on('focus', function () {

        $('#TextBox1').css('border-color', 'lightgrey');
    });

    $('#btnSearch').click(function () {
        $('#form0').submit();
    });

    $('#modDialog').on('show.bs.modal', function () {
        $('#modalName').text($('#introNameSong').text());
        $('#modalPerformer').text($('#introPerformer').text());
        $('#modalHiddenId').val($('#introSongId').val());
    })

    $('#button_prof').click(function () {
        var close = function () {
            $('.gb_Sa').css('display', 'none');
            $('.menu-profile-list').css('display', 'none');
            $('#button_prof').attr('aria-expanded', 'false');
        }

        if ($(this).attr('aria-expanded') === 'true') {
            close();
        }
        else {
            $('.gb_Sa').css('display', 'block');
            $('.menu-profile-list').css('display', 'block');
            $(this).attr('aria-expanded', 'true');
        }

        $('.main_div').click(function (e) {
                close();
        });
    });

});

$(function () {
    $.ajaxSetup({ cache: false });
    $("#modalImage").click(function (e) {

        e.preventDefault();
        $.get(this.href, function (data) {
            $('#dialogContent').html(data);
            $('#modDialog').modal('show');
        });
    });
})

function btnClick(obj, name, performer, songPath, imgPath) {

    song = new Audio(songPath);

    $('#introSongId').val($(obj).attr('id'));
    $('#introNameSong').text(name);
    $('#introPerformer').text(performer);
    $('#introImg').attr('src', imgPath);

    $('#btnDownload').attr('href', '/Home/Download/' + $(obj).attr('id'));

    if (!Song.paused) {
        $(Obj).css('color', '#0a0a0a');
        $(Obj).css('background', '#fff');

        Song.pause();

        if (obj != Obj) {
            Song = song;
            Obj = obj;

            Play();
        }
    }
    else {
        Song = song;
        Obj = obj;

        Play();
    }
}

function Play() {
    $(Obj).css('background', '#0a0a0a');
    $(Obj).css('color', '#fff');
    Page = $('.page__text').text();

    Song.play();
}

//$('.btn__link').click(function () {
//    if (!Song.paused && $('.page__text').text() == Page) {
//        alert($(Obj).attr('id'));

//        $($(Obj).attr('id')).css('background', '#0a0a0a');
//        $($(Obj).attr('id')).css('color', '#fff');
//    }
//});
let API_LIST_URL = '/api/catalogue';
let API_UPLOAD_URL = '/api/upload';
let MEDIA_BASE_URL = '/media/';

function api_listVideos() {
    return $.getJSON(API_LIST_URL);
}

function api_uploadFiles(files) {
    let form = new FormData();

    for (let file of files) {
        form.append('files', file);
    }

    return $.ajax({
        url: API_UPLOAD_URL,
        type: 'POST',
        data: form,
        processData: false,
        contentType: false
    });
}

$(function () {
    console.log('[home-index] script loaded');

    let $fileInput = $('#fileInput');
    let $btnUpload = $('#btnUpload');
    let $error = $('#errorMessage');
    let $tableBody = $('#tblVideos tbody');
    let $emptyState = $('#emptyState');
    let $player = $('#videoPlayer');

    let videos = [];

    function setError(text) {
        if (text) {
            $error.text(text).removeClass('d-none');
        } else {
            $error.text('').addClass('d-none');
        }
    }

    function setUploading(isUploading) {
        $btnUpload.prop('disabled', !!isUploading);
        $btnUpload.text(isUploading ? 'Uploading...' : 'Upload');
    }

    function updateEmptyState(items) {
        let hasVideos = items && items.length > 0;
        if (hasVideos) {
            $emptyState.addClass('d-none');
            $('#tblVideos').removeClass('d-none');
        } else {
            $emptyState.removeClass('d-none');
            $('#tblVideos').addClass('d-none');
        }
    }

    function renderTable(items) {
        $tableBody.empty();

        for (let v of items) {
            let $tr = $('<tr>')
                .append($('<td>').text(v.fileName))
                .append($('<td>').text(v.sizeKiloBytes))
                .data('video', v);

            $tableBody.append($tr);
        }
    }

    function loadVideos() {
        setError('');
        return api_listVideos()
            .done(function (resp) {
                videos = resp || [];
                renderTable(videos);
                updateEmptyState(videos);
            })
            .fail(function (xhr) {
                let msg = xhr?.responseText ? xhr.responseText : 'Failed to load videos.';
                setError(msg);
                videos = [];
                renderTable(videos);
                updateEmptyState(videos);
            });
    }

    function playVideo(video) {
        if (!video?.fileName) return;

        $('#tblVideos tr').removeClass('table-active');

        $('#tblVideos tr')
            .filter(function () {
                let v = $(this).data('video');
                return v && v.fileName === video.fileName;
            })
            .addClass('table-active');

        let src = MEDIA_BASE_URL + video.fileName;
        $player.attr('src', src);

        let el = $player.get(0);
        if (el) {
            el.load();
            el.play();
        }
    }

    $(document).on('click', '#tblVideos tbody tr', function () {
        const v = $(this).data('video');
        playVideo(v);
    });

    $btnUpload.on('click', function (e) {
        e.preventDefault();
        setError('');

        let input = $fileInput.get(0);
        let files = input?.files;
        if (!files || files.length === 0) {
            setError('Please choose one or more MP4 files.');
            return;
        }

        for (let file of files) {
            let name = (file.name || '').toLowerCase();
            if (!name.endsWith('.mp4')) {
                setError('Only MP4 files are allowed.');
                return;
            }
        }

        setUploading(true);
        api_uploadFiles(files)
            .done(function () {
                // Clear file input
                $fileInput.val('');

                // Refresh list and switch to catalogue
                loadVideos().always(function () {
                    let tabEl = document.querySelector('#catalogue-tab');
                    if (tabEl && globalThis.bootstrap && bootstrap.Tab) {
                        new bootstrap.Tab(tabEl).show();
                    }
                });
            })
            .fail(function (xhr) {
                let msg = xhr?.responseText ? xhr.responseText : 'Upload failed.';
                setError(msg);
            })
            .always(function () {
                setUploading(false);
            });
    });

    loadVideos();
});

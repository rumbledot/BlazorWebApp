function DownloadPDF(filename, byte_base64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + byte_base64;
    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
}

function ViewPDF(iFrame_id, byte_base64) {
    document.getElementById(iFrame_id).innerHTML = "";

    var iframe = document.createElement('iframe');
    iframe.setAttribute("src", "data:application/pdf;base64," + byte_base64);
    iframe.style.width = "100%";
    iframe.style.height = "680px";

    document.getElementById(iFrame_id).appendChild(iframe);
}

function Base64Blob(b64_data) {
    var slice_size = 512;
    var byte_char = atob(b64_data);
    var byte_arrays = [];

    for (var offset = 0; offset < byte_char.length; offset += slice_size) {
        var slice = byte_char.slice(offset, offset + slice_size);
        var byte_num = new Array(slice.length);

        for (var i = 0; i < slice.length; i++) {
            byte_num[i] = slice.charCodeAt(i);
        }

        var byte_array = new Uint8Array(byte_num);

        byte_arrays.push(byte_array);
    }

    var blob = new Blob(byte_arrays, { type: 'application/pdf' });
    return blob;
}
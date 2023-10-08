function displaySelectedFiles(event) {
    const previewDiv = document.getElementById('imagePreview');
    previewDiv.innerHTML = '';

    const files = event.target.files;
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            img.style.maxWidth = '100px';
            img.style.maxHeight = '100px';

            previewDiv.appendChild(img);
        };

        reader.readAsDataURL(file);
    }
}
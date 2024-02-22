window.initDragula = (netRefObj, ids, callbackName) => {
    const containers = [];
    for (let i = 0; i < ids.length; i++) {
        containers.push(document.querySelector(`#${ids[i]}`));
    }

    const drake = dragula(containers);

    drake.on('drop',
        (el, target, source, sibling) => {
            const uid = el.attributes["uid"].nodeValue;
            console.log(`${uid} dropped from ${source.id} to ${target.id}`);

            netRefObj.invokeMethodAsync(callbackName, uid, source.id, target.id);
        });
};

window.closeModal = (id) => {
    const modalElement = document.getElementById(id);
    const modal = window.bootstrap.Modal.getInstance(modalElement);

    modal.hide();
};

window.blazorCulture = {
        get : () => window.localStorage['BlazorCulture'],
        set : (value) => window.localStorage['BlazorCulture'] = value
    };
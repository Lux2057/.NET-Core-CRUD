window.ToDoSample = {
        Dragula : {
                init : (netRefObj, ids, callbackName) => {
                    const containers = [];
                    for (let i = 0; i < ids.length; i++) {
                        containers.push(document.querySelector(`#${ids[i]}`));
                    }

                    if (window.ToDoSample.Dragula._drake) {
                        window.ToDoSample.Dragula._drake.destroy();
                    }

                    window.ToDoSample.Dragula._drake = dragula(containers);

                    window.ToDoSample.Dragula._drake.on('drop',
                        (el, target, source, sibling) => {
                            const uid = el.attributes["uid"].nodeValue;
                            console.log(`${uid} dropped from ${source.id} to ${target.id}`);

                            netRefObj.invokeMethodAsync(callbackName, uid, source.id, target.id);
                        });
                }
            },
        Modal : {
                close : (id) => {
                    const modalElement = document.getElementById(id);
                    const modal = window.bootstrap.Modal.getInstance(modalElement);

                    modal.hide();
                }
            },
        LocalStorage : {
                get : (key) => window.localStorage[key],
                set : (key, value) => window.localStorage[key] = value
            }
    };
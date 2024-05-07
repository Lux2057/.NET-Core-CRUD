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

                    window.ToDoSample.Dragula._drake
                        .on('drop',
                            (el, target, source, sibling) => {
                                const uid = el.attributes["uid"].nodeValue;
                                console.log(`${uid} dropped from ${source.id} to ${target.id}`);

                                netRefObj.invokeMethodAsync(callbackName, uid, source.id, target.id);
                            })
                        .on('shadow',
                            (el, container, source) => {
                                if (el !== container.children[container.children.length - 1]) {
                                    container.appendChild(el);
                                }
                            });
                }
            },
        Modal : {
                close : (id) => {
                    const modalElement = document.getElementById(id);
                    const modal = window.bootstrap.Modal.getInstance(modalElement);

                    if (modal != null) {
                        modal.hide();
                    }
                    else {
                        const modals = document.getElementsByClassName("modal show");
                        if (modals.length) {
                            for (let i = 0; i < modals.length; i++) {
                                modals[i].classList.remove("show");
                            }
                        }

                        const backdrop = document.getElementsByClassName("modal-backdrop")[0];
                        backdrop.remove();

                        const body = document.getElementsByTagName("body")[0];
                        body.classList.remove("modal-open");
                        body.removeAttribute("style");
                    }

                }
            },
        LocalStorage : {
                get : (key) => window.localStorage[key],
                set : (key, value) => window.localStorage[key] = value
            }
    };
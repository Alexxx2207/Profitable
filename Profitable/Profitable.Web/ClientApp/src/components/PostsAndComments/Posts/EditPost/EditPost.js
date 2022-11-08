import { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { AuthContext } from "../../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../../contexts/MessageBoxContext";
import { ErrorWidget } from "../../../Common/ErrorWidget/ErrorWidget";

import {
    JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE,
    MISSING_POST_GUID_ERROR_PAGE_PATH,
    POST_CONTENT_MAX_COUNT,
    REQUESTER_NOT_OWNER_FRIENDLIER_MESSAGE,
    REQUESTER_NOT_OWNER_MESSAGE,
} from "../../../../common/config";
import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from "../../../../common/config";
import { getUserEmailFromJWT } from "../../../../services/users/usersService";
import {
    isEmptyOrWhiteSpaceFieldChecker,
    maxLengthChecker,
} from "../../../../services/common/errorValidationCheckers";
import { changeStateValuesForControlledForms } from "../../../../services/common/createStateValues";
import {
    createClientErrorObject,
    createServerErrorObject,
} from "../../../../services/common/createValidationErrorObject";
import { editPost, loadParticularPost } from "../../../../services/posts/postsService";
import { validateImage } from "../../../../services/common/imageService";

import styles from "./EditPost.module.css";

export const EditPost = () => {
    const { JWT, removeAuth } = useContext(AuthContext);

    const navigate = useNavigate();

    const { postId } = useParams();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [editState, setEditState] = useState({
        userEmail: "",
        values: {
            title: "",
            content: "",
            image: "",
            imageFileName: "",
        },
        errors: {
            titleEmpty: {
                text: "Insert title\\n(no whitespaces)",
                fulfilled: true,
                type: CLIENT_ERROR_TYPE,
            },
            contentEmpty: {
                text: "Insert main content\\n(no whitespaces)",
                fulfilled: true,
                type: CLIENT_ERROR_TYPE,
            },
            contentTooBig: {
                text: `Content less than ${POST_CONTENT_MAX_COUNT} characters`,
                fulfilled: true,
                type: CLIENT_ERROR_TYPE,
            },
            serverError: {
                text: "",
                display: false,
                type: SERVER_ERROR_TYPE,
            },
        },
    });

    const [removeImage, setRemoveImage] = useState(false);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((result) =>
                setEditState((state) => ({
                    ...state,
                    userEmail: result,
                }))
            )
            .catch((err) => {
                setMessageBoxSettings("You should login before editing a post!", false);
                removeAuth();
                navigate("/login");
            });
        // eslint-disable-next-line
    }, []);

    useEffect(() => {
        loadParticularPost(postId, editState.userEmail)
            .then((result) =>
                setEditState((state) => ({
                    ...state,
                    values: {
                        title: result.title,
                        content: result.content,
                        image: result.postImage,
                        imageFileName: result.postImageFileName,
                    },
                }))
            )
            .catch((err) => navigate(`${MISSING_POST_GUID_ERROR_PAGE_PATH}`));
    }, [postId, navigate, editState.userEmail]);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(editState.errors).filter(
            (err) => err.type === CLIENT_ERROR_TYPE
        );

        if (clientErrors.filter((err) => !err.fulfilled).length === 0) {
            let body = { ...editState.values };
            console.log(body);
            if (removeImage) {
                body.image = "";
                body.imageFileName = "";
            }

            editPost(JWT, postId, body)
                .then(() => {
                    setMessageBoxSettings("The post was edited successfully!", true);
                    navigate(`/posts/${postId}`);
                })
                .catch((err) => {
                    if (JWT && err.message === "Should auth first") {
                        setMessageBoxSettings(
                            JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE + " The post was not edited!",
                            false
                        );
                        removeAuth();
                        navigate("/login");
                    } else if (JWT && err.message === REQUESTER_NOT_OWNER_MESSAGE) {
                        setEditState((state) => ({
                            ...state,
                            errors: {
                                ...state.errors,
                                serverError: createServerErrorObject(
                                    REQUESTER_NOT_OWNER_FRIENDLIER_MESSAGE,
                                    true
                                ),
                            },
                        }));
                    } else {
                        setEditState((state) => ({
                            ...state,
                            errors: {
                                ...state.errors,
                                serverError: createServerErrorObject(err.message, true),
                            },
                        }));
                    }
                });
        }
    };

    const changeHandler = (e) => {
        if (e.target.name === "title") {
            setEditState((state) => ({
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    titleEmpty: createClientErrorObject(
                        state.errors.titleEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)
                    ),
                },
            }));
        } else if (e.target.name === "content") {
            setEditState((state) => ({
                ...state,
                values: changeStateValuesForControlledForms(
                    state.values,
                    e.target.name,
                    e.target.value
                ),
                errors: {
                    ...state.errors,
                    contentEmpty: createClientErrorObject(
                        state.errors.contentEmpty,
                        isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)
                    ),
                    contentTooBig: createClientErrorObject(
                        state.errors.contentTooBig,
                        maxLengthChecker.bind(null, e.target.value, POST_CONTENT_MAX_COUNT)
                    ),
                },
            }));
        } else if (e.target.name === "removeImage") {
            setRemoveImage((state) => e.target.checked);
        }
    };

    const changeImageHandler = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();

        if (validateImage(file, setMessageBoxSettings)) {
            const base64 = "base64,";

            reader.onloadend = () => {
                const base64Image = reader.result.toString();
                const byteArray = base64Image.slice(base64Image.indexOf(base64) + base64.length);

                const valuesObjectWithImage = changeStateValuesForControlledForms(
                    editState.values,
                    "image",
                    byteArray
                );
                const newValuesObjectWithImageFileName = changeStateValuesForControlledForms(
                    valuesObjectWithImage,
                    "imageFileName",
                    file.name
                );

                setEditState((state) => ({
                    ...state,
                    values: newValuesObjectWithImageFileName,
                }));
            };

            reader.readAsDataURL(file);
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.editSectionContainer}>
                <div className={styles.editContainer}>
                    <form className={styles.editForm} onSubmit={onSubmit}>
                        <div className={styles.editLabelContainer}>
                            <h1 className={styles.editLabel}>Edit Post</h1>
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Title</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                type="text"
                                name="title"
                                placeholder={"The Fed Rising Interest Rates"}
                                value={editState.values.title}
                                onChange={changeHandler}
                            />
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Main Content</h5>
                            </div>
                            <textarea
                                className={styles.inputField}
                                rows={5}
                                name="content"
                                placeholder={"While economists are worrying about the economy..."}
                                defaultValue={editState.values.content}
                                onChange={changeHandler}
                            />
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Image</h5>
                            </div>
                            <input
                                className={styles.inputField}
                                type="file"
                                name="image"
                                accept="image/*"
                                onChange={changeImageHandler}
                            />
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Remove Image</h5>
                            </div>
                            <input
                                className={styles.inputCheckbox}
                                type="checkbox"
                                defaultChecked={removeImage}
                                name="removeImage"
                                onChange={changeHandler}
                            />
                        </div>
                        <div className={styles.submitButtonContainer}>
                            <input className={styles.submitButton} type="submit" value="Edit" />
                        </div>
                    </form>
                </div>
                <aside className={styles.editAside}>
                    <div className={styles.errorsHeadingContainer}>
                        <h1 className={styles.errorsHeading}>Edit State</h1>
                    </div>
                    <div className={styles.errorsContainer}>
                        {Object.values(editState.errors).map((error, index) => (
                            <ErrorWidget key={index} error={error} />
                        ))}
                    </div>
                </aside>
            </div>
        </div>
    );
};

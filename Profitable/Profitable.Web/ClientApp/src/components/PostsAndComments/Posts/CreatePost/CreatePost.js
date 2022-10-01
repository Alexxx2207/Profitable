import { useContext, useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import classnames from "classnames";
import { AuthContext } from '../../../../contexts/AuthContext';
import { MessageBoxContext } from '../../../../contexts/MessageBoxContext';
import { createPost } from '../../../../services/posts/postsService';
import { ErrorWidget } from '../../../ErrorWidget/ErrorWidget';

import { CLIENT_ERROR_TYPE, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, SERVER_ERROR_TYPE } from '../../../../common/config';
import { getUserEmailFromJWT } from '../../../../services/users/usersService';
import { isEmptyOrWhiteSpaceFieldChecker } from '../../../../services/common/errorValidationCheckers';
import { changeStateValuesForControlledForms } from '../../../../services/common/createStateValues';
import { createClientErrorObject, createServerErrorObject } from '../../../../services/common/createValidationErrorObject';
import { validateImage } from '../../../../services/common/imageService';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowCircleLeft } from '@fortawesome/free-solid-svg-icons';

import styles from './CreatePost.module.css';

export const CreatePost = () => {

    const { JWT, removeAuth } = useContext(AuthContext);

    const navigate = useNavigate();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [createState, setCreateState] = useState({
        values: {
            title: '',
            content: '',
            image: '',
            imageFileName: '',
        },
        errors: {
            titleEmpty: { text: 'Insert title\\n(no whitespaces)', fulfilled: false, type: CLIENT_ERROR_TYPE },
            contentEmpty: { text: 'Insert main content\\n(no whitespaces)', fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    });

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(result => result)
            .catch(err => {
                setMessageBoxSettings('You should login before creating a post!', false);
                removeAuth();
                navigate('/login');
            })
        // eslint-disable-next-line
    }, []);

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(createState.errors).filter(err => err.type === CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            createPost(
                JWT, { ...createState.values }
            )
                .then(jwt => {
                    setMessageBoxSettings('The post was created successfully!', true);
                    navigate('/posts');
                })
                .catch(err => {
                    if (JWT && err.message === 'Should auth first') {
                        setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE + ' The post was not created!', false);
                        removeAuth();
                        navigate('/login');
                    } else {
                        setCreateState(state => ({
                            ...state,
                            errors: {
                                ...state.errors,
                                serverError: createServerErrorObject(err.message, true),
                            }
                        }));
                    }
                });
        }
    }

    const changeHandler = (e) => {
        if (e.target.name === 'title') {
            setCreateState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    titleEmpty: createClientErrorObject(state.errors.titleEmpty, isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)),
                }
            }));
        } else if (e.target.name === 'content') {
            setCreateState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    contentEmpty: createClientErrorObject(state.errors.contentEmpty, isEmptyOrWhiteSpaceFieldChecker.bind(null, e.target.value)),
                }
            }));
        }
    };

    const changeImageHandler = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();
        
        if(validateImage(file, setMessageBoxSettings)) {
            const base64 = 'base64,';

            reader.onloadend = () => {
                const base64Image = reader.result.toString();
                const byteArray = base64Image.slice(base64Image.indexOf(base64) + base64.length);

                const valuesObjectWithImage = changeStateValuesForControlledForms(createState.values, 'image', byteArray);
                const newValuesObjectWithImageFileName = changeStateValuesForControlledForms(valuesObjectWithImage, 'imageFileName', file.name);

                setCreateState(state => ({
                    ...state,
                    values: newValuesObjectWithImageFileName,
                }));
            }

            reader.readAsDataURL(file);
        }
    };

    return (
        <div className={styles.pageContainer}>
            <div className={styles.createSectionContainer}>
                <div className={styles.createContainer}>
                    <form className={styles.createForm} onSubmit={onSubmit} >
                        <div className={styles.createLabelContainer}>
                            <h1 className={styles.createLabel}>Create Post</h1>
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Title</h5>
                            </div>
                            <input className={styles.inputField} type="text" name="title" placeholder={'The Fed Rising Interest Rates'} value={createState.values.title} onChange={changeHandler} />
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Main Content</h5>
                            </div>
                            <textarea className={styles.inputField} name="content" placeholder={'While economists are worrying about the economy...'} defaultValue={createState.values.content} onChange={changeHandler} />
                        </div>
                        <div className={styles.formGroup}>
                            <div>
                                <h5>Image</h5>
                            </div>
                            <input className={styles.inputField} type="file" name="image" accept="image/*" onChange={changeImageHandler} />
                        </div>
                        <div className={styles.submitButtonContainer}>
                            <input className={styles.submitButton} type="submit" value='Create' />
                        </div>
                    </form>
                </div>
                <aside className={styles.createAside}>
                    <div className={styles.errorsHeadingContainer}>
                        <h1 className={styles.errorsHeading}>Create State</h1>
                    </div>
                    <div className={styles.errorsContainer}>
                        {Object.values(createState.errors).map((error, index) => <ErrorWidget key={index} error={error} />)}
                    </div>
                </aside>
            </div>
        </div>
    );
}
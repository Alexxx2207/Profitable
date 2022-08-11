import { useContext, useState } from 'react';
import { AuthContext } from '../../../../contexts/AuthContext';
import { useNavigate } from "react-router-dom"
import { createPost } from '../../../../services/posts/postsService';
import { ErrorWidget } from '../../../ErrorWidget/ErrorWidget';

import { CLIENT_ERROR_TYPE, SERVER_ERROR_TYPE } from '../../../../common/config';
import { isEmptyFieldChecker } from '../../../../services/common/errorValidationCheckers';
import { changeStateValuesForControlledForms } from '../../../../services/common/createStateValues';
import { createClientErrorObject, createServerErrorObject } from '../../../../services/common/createValidationErrorObject';

import styles from './CreatePost.module.css';

export const CreatePost = () => {

    const { JWT } = useContext(AuthContext);

    const navigate = useNavigate();

    const [createState, setCreateState] = useState({
        values: {
            title: '',
            content: '',
            image: '',
            imageFileName: '',
            author: '',
        },
        errors: {
            titleEmpty: { text: 'Insert title', fulfilled: false, type: CLIENT_ERROR_TYPE },
            contentEmpty: { text: 'Insert main content', fulfilled: false, type: CLIENT_ERROR_TYPE },
            serverError: {
                text: '',
                display: false,
                type: SERVER_ERROR_TYPE
            }
        }
    });

    const onSubmit = (e) => {
        e.preventDefault();

        const clientErrors = Object.values(createState.errors).filter(err => err.type === CLIENT_ERROR_TYPE);

        if (clientErrors.filter(err => !err.fulfilled).length === 0) {
            createPost(
                JWT, { ...createState.values }
            )
                .then(jwt => {
                    navigate('/posts');
                })
                .catch(err => {
                    if(err.message === 'Should auth first') {
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
        if(e.target.name === 'title') {
            setCreateState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    titleEmpty: createClientErrorObject(state.errors.titleEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                }
            }));
        } else  if(e.target.name === 'content'){
            setCreateState(state => ({
                ...state,
                values: changeStateValuesForControlledForms(state.values, e.target.name, e.target.value),
                errors: {
                    ...state.errors,
                    contentEmpty: createClientErrorObject(state.errors.contentEmpty, isEmptyFieldChecker.bind(null, e.target.value)),
                }
            }));
        }
    };

    const changeImageHandler = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();

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
    };

    return (
        <div className={styles.pageContainer}>
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
    );
}
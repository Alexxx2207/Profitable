import { useContext, useEffect, useReducer } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../../../contexts/AuthContext';
import { getUserDataByJWT } from '../../../../services/users/usersService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { postComment } from '../../../../services/comments/commentsService';
import { COMMENTS_LIST_IN_POST_PAGE_COUNT } from '../../../../common/config';

import styles from './CreateComment.module.css';
import { MessageBoxContext } from '../../../../contexts/MessageBoxContext';

const reducer = (state, action) => {
    switch (action.type) {
        case 'changeCommentContent':
            return {
                ...state,
                content: action.CommentContent
            };
        case 'changeCommentAuthor':
            return {
                ...state,
                authorName: action.authorName,
                authorEmail: action.authorEmail,
            };
        default:
            break;
    }
}

export const CreateComment = ({postId, loadFirstComments, handleAddCommentButton}) => {
    
    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [commentInfo, setCommentInfo] = useReducer(reducer, {
        authorName: '',
        authorEmail: '',
        content: ''
    });

    useEffect(() => {
        getUserDataByJWT(JWT)
            .then(result =>{
                setCommentInfo({
                    type: 'changeCommentAuthor',
                    authorName: result.firstName + " " + result.lastName,
                    authorEmail: result.email,
                })
            })
    }, [JWT]);

    const profileOpen = () => {
        navigate(`/users/${commentInfo.authorEmail}`);
    }

    const handleChange = (e) => {
        setCommentInfo({
            type: 'changeCommentContent',
            CommentContent: e.target.value
        })
    }
    const sendCommentHandler = (e) => {
        postComment(JWT, postId, commentInfo.content)
            .then(response => {
                setMessageBoxSettings('You posted a comment', true);
                loadFirstComments();
                handleAddCommentButton();
                setCommentInfo({
                    type: 'changeCommentContent',
                    CommentContent: ''
                })
            })
            .catch(err => {
                if(err.message === '401') {
                    setMessageBoxSettings('You should login first before adding a comment', false);
                    navigate(`/login`);
                } else if (err.message === '400') {
                    setMessageBoxSettings('Comment\'s length must be no longer 1000 symbols', false);
                }
            })
    }

    return (
        <div className={styles.commentWidget}>
            <header className={styles.commentHeader}>
                <h3 className={styles.commentAuthorName} onClick={() => profileOpen()}>{commentInfo.authorName}</h3>
                <h6 className={styles.commentAuthorEmail} onClick={() => profileOpen()}>({commentInfo.authorEmail})</h6>
            </header>
            <div className={styles.commentContentContainer}>
                <textarea
                className={styles.commentContent}
                value={commentInfo.content}
                onChange={handleChange}
                rows={10}
                placeholder={"Give your opinion here..."}></textarea>
            </div>

            <div className={styles.buttonSendContainer}>
                <button className={styles.buttonSend} onClick={sendCommentHandler}>
                    <FontAwesomeIcon icon={faPaperPlane} />
                    Send
                </button>
            </div>
        </div>
    );
}
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComments } from '@fortawesome/free-solid-svg-icons';
import { useNavigate } from 'react-router-dom';
import { createAuthorImgURL } from '../../../../../services/posts/postsService';
import { PostsLikeWidget } from '../../PostsLikeWidget/PostsLikeWidget';
import styles from './PostsListItem.module.css';

export const PostsListItem = (props) => {

    const navigate = useNavigate();

    const postDetailsClickHandler = () => {
        navigate(`/posts/${props.guid}`, { replace: false });
    }

    return (
        <div className={styles.post} onClick={postDetailsClickHandler}>
            <div className={styles.header}>
                <h2 className={styles.title}>{props.title}</h2>
                <PostsLikeWidget likesCount={props.likes.length} postId={props.guid} />
            </div>

            <div className={styles.mainContent}>
                <p className={styles.mainContent}>{props.content}</p>
            </div>

            <div className={styles.information}>
                <div className={styles.author}>
                    <img className={styles.authorImage} src={createAuthorImgURL(props.authorImageType, props.authorImage)} alt="" />
                    {props.author}
                </div>
                <div className={styles.author}>
                    <FontAwesomeIcon className={styles.iconComments} icon={faComments} />
                    Comments: <strong>{props.comments.length}</strong>
                </div>
                <div className={styles.postedOn}>
                    {props.postedOn}
                </div>
            </div>
        </div>);
}
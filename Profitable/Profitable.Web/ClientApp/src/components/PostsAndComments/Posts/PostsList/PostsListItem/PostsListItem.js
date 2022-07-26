import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faComments } from '@fortawesome/free-solid-svg-icons';
import styles from './PostsListItem.module.css';
import { useNavigate } from 'react-router-dom';

export const PostsListItem = (props) => {

    const navigate = useNavigate();

    const postClickHandler = () => {
        navigate(`/posts/${props.guid}`, { replace: false });
    }

    return (
        <div className={styles.post} onClick={postClickHandler}>
            <div className={styles.titleContent}>
                <h2>{props.title}</h2>

            </div>
            
            <div className={styles.mainContent}>
                <p className={styles.mainContent}>{props.content}</p>
            </div>

            <div className={styles.information}>
                <div className={styles.author}>
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
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faComments } from '@fortawesome/free-solid-svg-icons';
import styles from './PostsListItem.module.css';

export const PostsListItem = (props) => {
    return (<div className={styles.post}>
        <div className={styles.titleContent}>
            <h2>{props.title}</h2>
            <p className={styles.content}>{props.content}</p>
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
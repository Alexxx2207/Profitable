import { CLIENT_ERROR_TYPE } from '../../common/config';

import styles from './ErrorWidget.module.css';

export const ErrorWidget = ({error}) => {
    return (
        <div className={styles.errorContainer}>
            {error.type == CLIENT_ERROR_TYPE ?
                (error.fulfilled ? 
                    <h5 className={styles.errorMessageGreen}>{error.text}</h5>
                :
                    <h5 className={styles.errorMessageRed}>{error.text}</h5>
                )
            :
                (error.display ?
                    <h5 className={styles.errorMessageYellow}>{error.text}</h5>
                :
                    ''
                )
            }
        </div>
    );
}
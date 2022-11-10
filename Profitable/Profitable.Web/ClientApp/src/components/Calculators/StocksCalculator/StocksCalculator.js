import { useReducer, useContext, useEffect } from "react";
import classnames from "classnames";

import { AuthContext } from "../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";

import { calculateStocksTrade } from "../../../services/stocks/stocksService";

import styles from "./StocksCalculator.module.css";
import { useNavigate } from "react-router-dom";
import { InstrumentTypes, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE } from "../../../common/config";
import { getUserEmailFromJWT } from "../../../services/users/usersService";

const initialState = {
    userEmail: "",
    position: {
        entryPrice: 0,
        exitPrice: 0,
        entryCommission: 0,
        exitCommission: 0,
        numberOfShares: 0,
    },
    answer: {
        USDValue: 0,
    },
};

function stocksReducer(state, action) {
    switch (action.type) {
        case "setUserEmail":
            return {
                ...state,
                userEmail: action.payload,
            };
        case "loadAllStocks":
            return {
                ...state,
                allStocks: action.allStocks,
                chosenStocks: action.allStocks[0],
            };
        case "selectShareEntryPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryPrice: action.entryPrice,
                },
            };
        case "selectShareExitPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitPrice: action.exitPrice,
                },
            };
        case "selectShareEntryCommission":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryCommission: action.entryCommission,
                },
            };
        case "selectShareExitCommission":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitCommission: action.exitCommission,
                },
            };
        case "selectSharesCount":
            if (action.numberOfShares >= 0) {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfShares: action.numberOfShares,
                    },
                };
            } else {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfShares: 0,
                    },
                };
            }
        case "setResult":
            return {
                ...state,
                answer: {
                    USDValue: action.USDValue,
                },
            };
        default:
            break;
    }
}

export const StocksCalculator = () => {

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(stocksReducer, initialState);

    const navigate = useNavigate();

    useEffect(() => {
        if(JWT) {
            getUserEmailFromJWT(JWT)
            .then(userEmail => {
                setState({type: "setUserEmail", payload: userEmail});
            })
            .catch(err => {
                setMessageBoxSettings(JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, false);
                removeAuth();
                navigate("/login");
            })
        }
    }, []);

    const entryPriceOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectShareEntryPrice", entryPrice: e.target.value });
    };

    const exitPriceOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectShareExitPrice", exitPrice: e.target.value });
    };

    const entryCommissionOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectShareEntryCommission", entryCommission: e.target.value });
    };

    const exitCommissionOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectShareExitCommission", exitCommission: e.target.value });
    };

    const sharesNumberOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectSharesCount", numberOfShares: e.target.value });
    };

    const CalculatePLResult = (e) => {
        e.preventDefault();

        calculateStocksTrade(
            Number(state.position.entryPrice),
            Number(state.position.exitPrice),
            Number(state.position.numberOfShares),
            Number(state.position.entryCommission),
            Number(state.position.exitCommission)
        ).then((result) => {
            setState({ type: "setResult", USDValue: result.profitLoss });
        });
    };

    const handleSaveClick = (e) => {
        e.preventDefault();

        navigate("/positions/save",
        {
          state: {
            userEmail : state.userEmail,
            position: {...state.position, ...state.chosenFutures},
            instrumentGroup: InstrumentTypes.stocks
          }
        });
    }

    return (
        <div className={styles.stocksCalculatorWrapper}>
            <h2 className={styles.stocksCalculatorHeading}>Stocks Calculator</h2>
            <form className={styles.stocksCalculatorForm}>
                <div className={styles.stocksInformation}>
                    <div className={styles.positionInformationContainer}>
                        <div className={styles.entryPriceHeading}>
                            <h5>Entry Price</h5>
                        </div>
                        <div className={styles.entryPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => entryPriceOnChange(e)}
                                value={state.position.entryPrice}
                            />
                        </div>

                        <div className={styles.exitPriceHeading}>
                            <h5>Exit Price</h5>
                        </div>
                        <div className={styles.exitPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => exitPriceOnChange(e)}
                                value={state.position.exitPrice}
                            />
                        </div>

                        <div className={styles.sharesNumberHeading}>
                            <h5>Number of Share</h5>
                        </div>
                        <div className={styles.sharesNumberChange}>
                            <input
                                min={0}
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => sharesNumberOnChange(e)}
                                value={state.position.numberOfShares}
                            />
                        </div>
                    </div>
                    <div
                        className={classnames(
                            styles.positionInformationContainer,
                            styles.positionInformationCommisionContainer
                        )}
                    >
                        <div className={styles.entryPriceHeading}>
                            <h5>Buy Commission</h5>
                            <h6>In $</h6>
                        </div>
                        <div className={styles.entryPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => entryCommissionOnChange(e)}
                                value={state.position.entryCommission}
                            />
                        </div>

                        <div className={styles.exitPriceHeading}>
                            <h5>Sell Commission</h5>
                            <h6>In $</h6>
                        </div>
                        <div className={styles.exitPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => exitCommissionOnChange(e)}
                                value={state.position.exitCommission}
                            />
                        </div>
                    </div>
                </div>
            </form>
            <div className={styles.calculateButtonContainer}>
                <div className={styles.calculateButton} onClick={(e) => CalculatePLResult(e)}>
                    Calculate
                </div>
            </div>

            <div className={styles.answerWrapper}>
                <h3 className={styles.resultsLabel}>Profit or Loss Results</h3>
                <div className={styles.answerContainer}>
                    <div className={styles.dollarsHeading}>
                        <h5>USD($)</h5>
                    </div>
                    <div className={styles.dollarsValue}>
                        {state.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                -${Math.abs(state.answer.USDValue).toFixed(2)}
                            </h5>
                        ) : state.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>${state.answer.USDValue.toFixed(2)}</h5>
                        ) : (
                            <h5>${state.answer.USDValue.toFixed(2)}</h5>
                        )}
                    </div>
                </div>
                {
                    state.userEmail ?
                    <div className={styles.saveToPositionsContainer}>
                        <button onClick={handleSaveClick} className={styles.saveToRecordButton}>Save To Record</button>
                    </div>
                    :
                    <></>
                }
            </div>
        </div>
    );
};

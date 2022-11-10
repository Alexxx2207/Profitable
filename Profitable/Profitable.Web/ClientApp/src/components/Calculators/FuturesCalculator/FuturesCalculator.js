import { useContext, useEffect, useReducer } from "react";
import { useNavigate } from "react-router-dom";
import { InstrumentTypes, JWT_EXPIRED_WHILE_EDITING_ERROR_MESSAGE, LongDirectionValue, ShortDirectionValue } from "../../../common/config";

import { AuthContext } from "../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";

import { loadFuturesContracts, CalculatePosition } from "../../../services/futures/futuresService";
import { getUserEmailFromJWT } from "../../../services/users/usersService";

import styles from "./FuturesCalculator.module.css";

const initialState = {
    userEmail: "",
    allFutures: [],
    chosenFutures: {
        name: "",
        tickSize: 0,
        tickValue: 0,
    },
    position: {
        directionBullish: true,
        entryPrice: 0,
        exitPrice: 0,
        numberOfContracts: 0,
    },
    answer: {
        ticks: 0,
        USDValue: 0,
    },
};

function futuresReducer(state, action) {
    switch (action.type) {
        case "setUserEmail":
            return {
                ...state,
                userEmail: action.payload,
            };
        case "loadAllFutures":
            return {
                ...state,
                allFutures: action.allFutures,
                chosenFutures: action.allFutures[0],
            };
        case "selectContractType":
            return {
                ...state,
                chosenFutures: state.allFutures[action.index],
            };
        case "selectContractDirection":
            return {
                ...state,
                position: {
                    ...state.position,
                    directionBullish: action.directionBullish,
                },
            };
        case "selectContractEntryPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryPrice: action.entryPrice,
                },
            };
        case "selectContractExitPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitPrice: action.exitPrice,
                },
            };
        case "selectContractsCount":
            if (action.numberOfContracts >= 0) {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfContracts: action.numberOfContracts,
                    },
                };
            } else {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfContracts: 0,
                    },
                };
            }
        case "setResult":
            return {
                ...state,
                answer: {
                    ticks: action.ticks,
                    USDValue: action.USDValue,
                },
            };
        default:
            break;
    }
}

export const FuturesCalculator = () => {

    const { JWT, removeAuth } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [state, setState] = useReducer(futuresReducer, initialState);

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
        
        loadFuturesContracts().then((futuresContracts) =>
            setState({ type: "loadAllFutures", allFutures: futuresContracts })
        );
    }, []);

    const OnChangeContractType = (value) => {
        setState({ type: "selectContractType", index: value });
    };

    const onBullishDirectionButtonClick = (e) => {
        setState({ type: "selectContractDirection", directionBullish: true });
    };

    const onBearishDirectionButtonClick = (e) => {
        setState({ type: "selectContractDirection", directionBullish: false });
    };

    const entryPriceOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectContractEntryPrice", entryPrice: e.target.value });
    };

    const exitPriceOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectContractExitPrice", exitPrice: e.target.value });
    };

    const contractsNumberOnChange = (e) => {
        e.preventDefault();
        setState({ type: "selectContractsCount", numberOfContracts: e.target.value });
    };

    const CalculatePLResult = (e) => {
        e.preventDefault();

        CalculatePosition(
            state.position.directionBullish ? LongDirectionValue : ShortDirectionValue,
            Number(state.position.entryPrice),
            Number(state.position.exitPrice),
            Number(state.position.numberOfContracts),
            state.chosenFutures.tickSize,
            state.chosenFutures.tickValue
        ).then((result) => {
            setState({ type: "setResult", ticks: result.ticks, USDValue: result.profitLoss });
        });
    };

    const handleSaveClick = (e) => {
        e.preventDefault();

        navigate("/positions/save",
        {
          state: {
            userEmail : state.userEmail,
            position: {...state.position, ...state.chosenFutures},
            instrumentGroup: InstrumentTypes.futures
          }
        });
    }

    return (
        <div className={styles.futuresCalculatorWrapper}>
            <h2 className={styles.futuresCalculatorHeading}>Futures Calculator</h2>
            <form className={styles.futuresCalculatorForm}>
                <div className={styles.contractTypeContainer}>
                    <h4 className={styles.contractTypeHeading}>Select Contract</h4>
                    <select
                        className={styles.selectContract}
                        onChange={(e) => OnChangeContractType(e.target.value)}
                        select={state.chosenFutures.name}
                    >
                        {state.allFutures.map((futureContract, index) => (
                            <option
                                className={styles.selectContractOption}
                                key={futureContract.guid}
                                value={index}
                            >
                                {futureContract.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className={styles.futuresInformation}>
                    <div className={styles.positionInformationContainer}>
                        <div className={styles.directionsHeading}>
                            <h5>Position Direction</h5>
                        </div>
                        <div className={styles.directions}>
                            <div
                                className={styles.radioWrapper}
                                onClick={(e) => onBullishDirectionButtonClick(e)}
                            >
                                <input
                                    type="radio"
                                    onChange={(e) => onBullishDirectionButtonClick(e)}
                                    checked={state.position.directionBullish}
                                    className={styles.directionBullish}
                                    name="directionSelected"
                                />
                                <span
                                    className={styles.radioLabel}
                                    onClick={(e) => onBullishDirectionButtonClick(e)}
                                >
                                    Bullish
                                </span>
                            </div>
                            <div
                                className={styles.radioWrapper}
                                onClick={(e) => onBearishDirectionButtonClick(e)}
                            >
                                <input
                                    type="radio"
                                    onChange={(e) => onBearishDirectionButtonClick(e)}
                                    checked={!state.position.directionBullish}
                                    className={styles.directionBearish}
                                    name="directionSelected"
                                />
                                <span
                                    className={styles.radioLabel}
                                    onClick={(e) => onBearishDirectionButtonClick(e)}
                                >
                                    Bearish
                                </span>
                            </div>
                        </div>

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

                        <div className={styles.contractsNumberHeading}>
                            <h5>Number of Contract</h5>
                        </div>
                        <div className={styles.contractsNumberChange}>
                            <input
                                min={0}
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => contractsNumberOnChange(e)}
                                value={state.position.numberOfContracts}
                            />
                        </div>
                    </div>
                    <aside className={styles.contractInformationContainer}>
                        <div className={styles.contractNameContainer}>
                            <h5>{state.chosenFutures.name}</h5>
                        </div>
                        <div className={styles.tickSizeHeadingContainer}>
                            <h5>Tick Size</h5>
                        </div>
                        <div className={styles.tickSizeValueContainer}>
                            <h5>{state.chosenFutures.tickSize}</h5>
                        </div>
                        <div className={styles.tickValueHeadingContainer}>
                            <h5>Tick Value</h5>
                        </div>
                        <div className={styles.tickValueContainer}>
                            <h5>${state.chosenFutures.tickValue}</h5>
                        </div>
                    </aside>
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
                    <div className={styles.ticksHeading}>
                        <h5>Ticks</h5>
                    </div>
                    <div className={styles.ticksValue}>
                        {state.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                {Math.abs(state.answer.ticks).toFixed(2)}
                            </h5>
                        ) : state.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>
                                {Math.abs(state.answer.ticks).toFixed(2)}
                            </h5>
                        ) : (
                            <h5>{Math.abs(state.answer.ticks).toFixed(2)}</h5>
                        )}
                    </div>

                    <div className={styles.dollarsHeading}>
                        <h5>USD($)</h5>
                    </div>
                    <div className={styles.dollarsValue}>
                        {state.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                -${Math.abs(state.answer.USDValue).toFixed(2)}
                            </h5>
                        ) : state.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>
                                ${state.answer.USDValue.toFixed(2)}
                            </h5>
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

import React, { useState } from 'react';
// import emailjs from 'emailjs-com';
// import 'react-phone-number-input/style.css'
// import "react-phone-input-2/lib/style.css";
import './Style.css'

const Projecthead = () => {
    // const [value, setValue] = useState();
    // const [username, setusername] = useState('');
    // const [name, setname] = useState('');
    // const [email, setemail] = useState('');
    // const [phonenumber, setphonenumber] = useState('');

    // const handleSendEmail = (e) => {
    //     e.preventDefault();
    //     emailjs.sendForm('service_tnnc9wg', 'template_1l99o5c', e.target, 'EkqprxSWHhcvUSK7v')
    //         .then((result) => {
    //             window.location.reload();
    //         }, (error) => {
    //             console.log(error.text);
    //         });
    // };

    return (
        <>
            <div className="main">
                <div className="container">
                    <h1>Project Head</h1>
                    <form  className="form">
                        <table>
                            <tbody>
                                <tr>
                                    <td className="td-class">
                                        <label htmlFor="name">Vendor Name:</label>
                                    </td>
                                    <td>
                                        <input type="text" placeholder="Enter Name" name="name"  className="input-class" />
                                    </td>
                                </tr>
                                <tr>
                                    <td className="td-class">
                                        <label htmlFor="username">Username:</label>
                                    </td>
                                    <td>
                                        <input type="text" placeholder="Enter Username"  name="username" className="input-class" />
                                    </td>
                                </tr>
                                <tr>
                                    <td className="td-class">
                                        <label htmlFor="email">Email Id:</label>
                                    </td>
                                    <td>
                                        <input type="email" placeholder="Enter Email"  className="input-class" />
                                    </td>
                                </tr>
                                <tr>
                                    <td className="td-class">
                                        <label htmlFor="phonenumber">Phone Number:</label>
                                    </td>
                                    <td>
                                        <div className='Phonenumber'>
                                        <input type="tel" id="phone" name="phone" placeholder='Enter Phone Number'  className="input-class" pattern="[0-9]{3}[0-9]{3}[0-9]{4}" required  />
                                        </div>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td className="td-class" colSpan="2">
                                        <button type="submit" className="button">Submit</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </form>
                </div>
            </div>
        </>
    )
}

export default Projecthead;
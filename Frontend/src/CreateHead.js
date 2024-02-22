import React from "react";
import "./AddRegion.css"
import Header from "./Header";
import axios from 'axios';
import { useState } from "react";
import { useNavigate } from "react-router-dom";

function CreateHead(){

 
    const [formData, setFormData] = useState({
        name: '',
        code: '',
        regionImageUrl:''
      });

      const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData((prevFormData) => ({
          ...prevFormData,
          [name]: value, 
        }));
      };
    
      const handleSubmit = async (event) => {
        event.preventDefault();

        try {
          const response = await axios.post('https://localhost:7075/api/Auth/Register/ProjectHead',formData);
          console.log(response.data);
          alert(response.data)
        } 
        catch (error) {
          alert("you are not authorized user to perform this operation");
          console.error('There was a problem with the axios post request:', error);
        }
      };   
    

    return (
        <>
        <Header/>
        <div className="addRegion-table">
                <table className="table table-bordered">
                <tbody>
                    <tr>
                        <th>Name</th>
                        <td>
                            <input type="text" name="name" placeholder="Name"  onChange={handleInputChange}/>
                        </td>
                    </tr>         
                    <tr>
                        <th>UserName</th>
                        <td>
                            <input type="text" name="userName" placeholder="UserName"   onChange={handleInputChange} />
                        </td>
                    </tr>   
                    <tr>
                        <th>Email</th>
                        <td>
                            <input type="text" name="email" placeholder="Email" onChange={handleInputChange}/>
                        </td>
                    </tr>  
                    <tr>
                        <th>Phone</th>
                        <td>
                            <input type="text" name="phoneNumber" placeholder="Phone number" onChange={handleInputChange}/>
                        </td>
                    </tr>                        
                    <tr>
                        <th>Password</th>
                        <td>
                            <input type="text" name="password" placeholder="Password" onChange={handleInputChange}/>
                        </td>
                    </tr>    
                    <tr>
                        <th colSpan={2}>
                            <button className="btn btn-primary" onClick={handleSubmit}> Create Project Head</button>
                        </th>
                    </tr>                     
                </tbody>                   
                </table>
            </div>
        </>
    );
}

export default CreateHead;


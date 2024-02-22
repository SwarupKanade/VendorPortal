import './Style.css';

const Admin = () => {
    
  return (
    <div className="main">
    <div className="container">
    <h1>Admin</h1>
        <form>
          <table>
            <tbody>
                
              <tr>
                <td className="td-class">
                  <label htmlFor="id">Vendor Id:</label>
                </td >
                <td >
                  <input className="input-class" 
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="name">Vendor Name:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="name">Contact person:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="number">Mobile Number:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="number"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="email">Email Id:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="email"
                  />
                </td >
              </tr>
              
              <tr>
                <td className="td-class">
                  <label htmlFor="name"> Select Category:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="name">State:</label>
                </td >
                <td >
                  <input className="input-class"
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="name">City:</label>
                </td>
                <td >
                  <input className="input-class"
                    type="text"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="address">Address:</label>
                </td>
                <td >
                  <input className="input-class"
                    type="address"
                  />
                </td >
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="pin">Pin Code:</label>
                </td>
                <td >
                  <input className="input-class"
                    type="number"
                  />
                </td>
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="password ">Set Password:</label>
                </td>
                <td >
                  <input className="input-class"
                    type="password"/>
                </td>
              </tr>
             
              <tr>
                <td className="td-class" colSpan="2">
                  <button className="button" type="submit">Submit</button>
                </td >
              </tr>
            </tbody>
          </table>
        </form>

    </div>
    </div>
  );
};

export default Admin;
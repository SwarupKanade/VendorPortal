import { Link } from 'react-router-dom';
import './NavbarApp';


function Sidebar(){

   
    return(
        <div  >
           
          <div className="sideBar"  style={{height:900}}>
            <ul>
                <li>
                    <Link to="/Vendor" className='operation'>Create Vendor Account</Link>
                   
                </li>
                <li>
                    <Link to='/Projecthead' className='operation'>Create Project Head</Link>
                </li>
                <li>
                
                    <Link to='/Project' className='operation'>Create Project</Link>
                </li>
                <li>
                <Link to='/RfpRfqForm'  className='operation'>Create Request for Proposal</Link>
               
                </li>
            </ul>
          </div>
        </div>
    );
}
export default Sidebar;
import React, { useState, useEffect } from 'react';
import './Style.css'; // Import custom CSS file

const RfpRfqForm = () => {
  const [documentType, setDocumentType] = useState('');
  const [selectedFile, setSelectedFile] = useState(null);
  const [selectedProject, setSelectedProject] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [endDate, setEndDate] = useState('');
  const [projects, setProjects] = useState([]);
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    // Fetch projects from an API
    fetchProjects()
      .then(data => setProjects(data))
      .catch(error => console.error('Error fetching projects:', error));

    // Fetch categories from an API
    fetchCategories()
      .then(data => setCategories(data))
      .catch(error => console.error('Error fetching categories:', error));
  }, []);

  const fetchProjects = async () => {
    // Perform API call to fetch projects
    // Example:
    const response = await fetch('api/projects');
    const data = await response.json();
    return data;
  };

  const fetchCategories = async () => {
    // Perform API call to fetch categories
    // Example:
    const response = await fetch('api/categories');
    const data = await response.json();
    return data;
  };

  const handleFileChange = (event) => {
    const file = event.target.files[0];
    const maxSize = 10 *1024 * 1024; // 10MB
    const allowedTypes = ['application/pdf'];

    if (file.size > maxSize) {
      alert('File size exceeds 10MB.');
    } else if (!allowedTypes.includes(file.type)) {
      alert('Only PDF files are allowed.');
    } else {
      setSelectedFile(file);
    }
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (!selectedFile) {
      alert('Please upload a file.');
      return;
    }
    // Your form submission logic goes here

    // Example of success message
    alert('Form submitted successfully!');
  };

  return (
    <div className="main">
      <div className="container">
        <h1>RFP/RFQ</h1>
        <form onSubmit={handleSubmit}>
          <table>
            <tbody>
              <tr>
                <td className="td-class">
                  <label htmlFor="documentType">Document Type:</label>
                </td>
                <td>
                  <select className="input-class" value={documentType} onChange={(e) => setDocumentType(e.target.value)}>
                    <option value="">Select Document Type</option>
                    <option value="Technical Specification">Technical Specification</option>
                    <option value="Financial Proposal">Financial Proposal</option>
                    <option value="Company Profile">Company Profile</option>
                    <option value="Previous Work Examples">Previous Work Examples</option>
                    <option value="Other">Other</option>
                  </select>
                </td>
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="selectedFile">Document Upload:</label>
                </td>
                <td>
                  <input className="input-class" type="file" accept=".pdf" onChange={handleFileChange} />
                </td>
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="selectedProject">Select Project:</label>
                </td>
                <td>
                  <select className="input-class" value={selectedProject} onChange={(e) => setSelectedProject(e.target.value)}>
                    <option value="">Select Project</option>
                    {projects.map(project => (
                      <option key={project.id} value={project.id}>{project.name}</option>
                    ))}
                  </select>
                </td>
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="selectedCategory">Vendor Categories:</label>
                </td>
                <td>
                  <select className="input-class" value={selectedCategory} onChange={(e) => setSelectedCategory(e.target.value)}>
                    <option value="">Select Category</option>
                    {categories.map(category => (
                      <option key={category.id} value={category.id}>{category.name}</option>
                    ))}
                  </select>
                </td>
              </tr>
              <tr>
                <td className="td-class">
                  <label htmlFor="endDate">End Date to Apply:</label>
                </td>
                <td>
                  <input className="input-class" type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                </td>
              </tr>
              <tr>
                <td className="td-class" colSpan="2">
                  <button className="button" type="submit">Publish</button>
                </td>
              </tr>
            </tbody>
          </table>
        </form>
      </div>
    </div>
  );
};

export default RfpRfqForm;

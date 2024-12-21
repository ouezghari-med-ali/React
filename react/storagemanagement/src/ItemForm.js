import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const ItemForm = () => {
  const [item, setItem] = useState({
    name: '',
    quantity: 0,
    location: ''
  });
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const handleChange = (e) => {
    const { name, value } = e.target;
    setItem((prevItem) => ({ ...prevItem, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const token = localStorage.getItem('token');
      const response = await axios.post('https://localhost:7202/api/items', item, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      console.log('Item added:', response.data);
      alert('Item added successfully');
      setItem({ name: '', quantity: 0, location: '' });
    } catch (error) {
      console.error('Error adding item:', error);
      setError('Failed to add item. Please ensure you are logged in.');
    }
  };

  const handleGoBack = () => {
    navigate('/items-table');
  };

  return (
    <div className="container">
      <h2>Add Item</h2>
      {error && <p className="error">{error}</p>}
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label>Name:</label>
          <input
            type="text"
            name="name"
            value={item.name}
            onChange={handleChange}
            required
          />
        </div>
        <div className="input-group">
          <label>Quantity:</label>
          <input
            type="number"
            name="quantity"
            value={item.quantity}
            onChange={handleChange}
            required
          />
        </div>
        <div className="input-group">
          <label>Location:</label>
          <input
            type="text"
            name="location"
            value={item.location}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit">Add Item</button>
        <button type="button" onClick={handleGoBack} style={{ marginTop: '10px', backgroundColor: '#1890ff', color: 'white' }}>
          Go to Items Table
        </button>
      </form>
    </div>
  );
};

export default ItemForm;

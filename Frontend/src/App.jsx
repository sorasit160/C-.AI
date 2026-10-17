import { useState, useEffect, useMemo } from 'react';
import './App.css';
import { 
  Wallet, 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Plus, 
  Trash2, 
  Search, 
  RefreshCw, 
  PieChart, 
  Calendar,
  X,
  CreditCard,
  Tag,
  AlertCircle
} from 'lucide-react';

const API_BASE = 'http://localhost:5036/api';

const CATEGORY_NAMES = {
  Food: 'อาหารและเครื่องดื่ม',
  Transport: 'การเดินทาง / ค่าน้ำมัน',
  Shopping: 'ช้อปปิ้งของใช้',
  Bills: 'บิล ค่าน้ำ ค่าไฟ เน็ต',
  Entertainment: 'ท่องเที่ยว / สังสรรค์',
  Salary: 'เงินเดือน / ค่าจ้าง',
  Other: 'อื่นๆ ทั่วไป'
};

export default function App() {
  const [expenses, setExpenses] = useState([]);
  const [summary, setSummary] = useState(null);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [apiOnline, setApiOnline] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  
  // Modal state
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState({
    description: '',
    amount: '',
    category: 'Food',
    isIncome: false,
    date: new Date().toISOString().split('T')[0]
  });
  const [submitting, setSubmitting] = useState(false);

  // Load Data
  const fetchData = async () => {
    try {
      setLoading(true);
      const [expRes, sumRes, catRes] = await Promise.all([
        fetch(`${API_BASE}/expenses${searchTerm ? `?q=${encodeURIComponent(searchTerm)}` : ''}`),
        fetch(`${API_BASE}/expenses/summary`),
        fetch(`${API_BASE}/categories`)
      ]);

      if (expRes.ok && sumRes.ok && catRes.ok) {
        setExpenses(await expRes.json());
        setSummary(await sumRes.json());
        setCategories(await catRes.json());
        setApiOnline(true);
      } else {
        setApiOnline(false);
      }
    } catch (err) {
      console.error('Fetch error:', err);
      setApiOnline(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [searchTerm]);

  // Handle Add Item
  const handleCreate = async (e) => {
    e.preventDefault();
    if (!formData.description || !formData.amount) return;

    setSubmitting(true);
    try {
      const payload = {
        description: formData.description,
        amount: parseFloat(formData.amount),
        category: formData.isIncome ? 'Salary' : formData.category,
        isIncome: formData.isIncome,
        date: formData.date ? new Date(formData.date).toISOString() : new Date().toISOString()
      };

      const res = await fetch(`${API_BASE}/expenses`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      if (res.ok) {
        setIsModalOpen(false);
        setFormData({
          description: '',
          amount: '',
          category: 'Food',
          isIncome: false,
          date: new Date().toISOString().split('T')[0]
        });
        await fetchData();
      } else {
        const errorData = await res.json().catch(() => ({}));
        alert('บันทึกไม่สำเร็จ: ' + (errorData.message || res.statusText));
      }
    } catch (err) {
      alert('บันทึกไม่สำเร็จ: ' + err.message);
    } finally {
      setSubmitting(false);
    }
  };

  // Handle Delete
  const handleDelete = async (id) => {
    if (!window.confirm('คุณแน่ใจว่าต้องการลบรายการนี้?')) return;
    try {
      const res = await fetch(`${API_BASE}/expenses/${id}`, { method: 'DELETE' });
      if (res.ok) {
        await fetchData();
      }
    } catch (err) {
      alert('ลบไม่สำเร็จ: ' + err.message);
    }
  };

  const formatMoney = (val) => {
    return new Intl.NumberFormat('th-TH', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(val || 0);
  };

  return (
    <div className="app-container">
      {/* Header */}
      <header className="app-header">
        <div className="brand-section">
          <div className="brand-icon-wrapper">
            <Wallet size={28} />
          </div>
          <div>
            <h1 className="brand-title">Expense Tracker</h1>
            <p className="brand-subtitle">ระบบบริหารรายรับ-รายจ่ายส่วนบุคคล (.NET 10 + React)</p>
          </div>
        </div>

        <div className="header-actions">
          <div className={`status-badge ${apiOnline ? 'online' : 'offline'}`}>
            <span className="status-dot"></span>
            {apiOnline ? 'API เชื่อมต่อแล้ว (5036)' : 'API ออฟไลน์'}
          </div>

          <button className="btn btn-refresh" onClick={fetchData} title="รีเฟรชข้อมูล">
            <RefreshCw size={16} />
          </button>

          <button className="btn btn-primary" onClick={() => setIsModalOpen(true)}>
            <Plus size={18} />
            <span>เพิ่มรายการใหม่</span>
          </button>
        </div>
      </header>

      {/* API offline warning */}
      {!apiOnline && (
        <div style={{
          background: 'rgba(239, 68, 68, 0.15)',
          border: '1px solid rgba(239, 68, 68, 0.3)',
          padding: '1rem',
          borderRadius: '12px',
          marginBottom: '1.5rem',
          display: 'flex',
          alignItems: 'center',
          gap: '0.75rem',
          color: '#fca5a5'
        }}>
          <AlertCircle size={20} />
          <span>กรุณาตรวจสอบว่า Backend API กำลังทำงานอยู่ที่ <code>http://localhost:5036</code> (คำสั่ง: <code>dotnet run --project ExpenseTracker.Api</code>)</span>
        </div>
      )}

      {/* Stats Summary Cards */}
      <section className="stat-grid">
        <div className="stat-card balance">
          <div className="stat-icon balance">
            <DollarSign size={24} />
          </div>
          <div>
            <div className="stat-label">ยอดคงเหลือสุทธิ</div>
            <div className="stat-value balance">฿{formatMoney(summary?.balance)}</div>
          </div>
        </div>

        <div className="stat-card income">
          <div className="stat-icon income">
            <TrendingUp size={24} />
          </div>
          <div>
            <div className="stat-label">รายรับทั้งหมด</div>
            <div className="stat-value income">+฿{formatMoney(summary?.totalIncome)}</div>
          </div>
        </div>

        <div className="stat-card expense">
          <div className="stat-icon expense">
            <TrendingDown size={24} />
          </div>
          <div>
            <div className="stat-label">รายจ่ายทั้งหมด</div>
            <div className="stat-value expense">-฿{formatMoney(summary?.totalExpense)}</div>
          </div>
        </div>
      </section>

      {/* Main Grid: Transactions & Analytics */}
      <main className="dashboard-grid">
        {/* Left: Transactions List */}
        <section className="panel">
          <div className="panel-header">
            <h2 className="panel-title">
              <CreditCard size={20} color="#818cf8" />
              ประวัติรายการล่าสุด ({expenses.length})
            </h2>

            <div className="search-box">
              <Search size={16} color="var(--text-dim)" />
              <input 
                type="text" 
                placeholder="ค้นหารายการ..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </div>

          <div className="tx-list">
            {expenses.length === 0 ? (
              <div className="empty-state">
                <CreditCard size={40} />
                <p>ยังไม่มีรายการบันทึก</p>
                <button 
                  className="btn btn-primary" 
                  style={{ marginTop: '1rem' }}
                  onClick={() => setIsModalOpen(true)}
                >
                  เริ่มเพิ่มรายการแรก
                </button>
              </div>
            ) : (
              expenses.map((item) => (
                <div className="tx-item" key={item.id}>
                  <div className="tx-info">
                    <span className="tx-badge-cat">
                      {CATEGORY_NAMES[item.category] || item.category}
                    </span>
                    <div className="tx-details">
                      <span className="tx-desc">{item.description}</span>
                      <span className="tx-date">
                        {new Date(item.date).toLocaleDateString('th-TH', {
                          year: 'numeric',
                          month: 'short',
                          day: 'numeric'
                        })}
                      </span>
                    </div>
                  </div>

                  <div className="tx-actions">
                    <span className={`tx-amount ${item.isIncome ? 'income' : 'expense'}`}>
                      {item.isIncome ? '+' : '-'}฿{formatMoney(item.amount)}
                    </span>
                    <button 
                      className="btn-icon-del" 
                      onClick={() => handleDelete(item.id)}
                      title="ลบรายการ"
                    >
                      <Trash2 size={16} />
                    </button>
                  </div>
                </div>
              ))
            )}
          </div>
        </section>

        {/* Right: Breakdown & Insights */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1.75rem' }}>
          {/* Category Breakdown */}
          <section className="panel">
            <div className="panel-header">
              <h2 className="panel-title">
                <PieChart size={20} color="#ec4899" />
                สัดส่วนรายจ่ายตามหมวดหมู่
              </h2>
            </div>

            <div className="breakdown-list">
              {summary?.expenseByCategory?.length === 0 ? (
                <p style={{ color: 'var(--text-dim)', textAlign: 'center', padding: '1rem' }}>
                  ยังไม่มีข้อมูลรายจ่าย
                </p>
              ) : (
                summary?.expenseByCategory?.map((cat) => {
                  const pct = summary.totalExpense > 0 
                    ? ((cat.total / summary.totalExpense) * 100).toFixed(1) 
                    : 0;

                  return (
                    <div className="breakdown-row" key={cat.category}>
                      <div className="breakdown-info">
                        <span>{CATEGORY_NAMES[cat.category] || cat.category}</span>
                        <span>฿{formatMoney(cat.total)} ({pct}%)</span>
                      </div>
                      <div className="bar-bg">
                        <div className="bar-fill" style={{ width: `${pct}%` }}></div>
                      </div>
                    </div>
                  );
                })
              )}
            </div>
          </section>

          {/* Monthly Breakdown */}
          <section className="panel">
            <div className="panel-header">
              <h2 className="panel-title">
                <Calendar size={20} color="#10b981" />
                สรุปรายเดือน
              </h2>
            </div>

            <div className="breakdown-list">
              {summary?.monthlySummary?.length === 0 ? (
                <p style={{ color: 'var(--text-dim)', textAlign: 'center', padding: '1rem' }}>
                  ยังไม่มีข้อมูลรายเดือน
                </p>
              ) : (
                summary?.monthlySummary?.map((m, idx) => (
                  <div key={idx} style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    padding: '0.65rem 0.85rem',
                    background: 'rgba(15, 23, 42, 0.45)',
                    borderRadius: '8px',
                    fontSize: '0.85rem'
                  }}>
                    <span style={{ fontWeight: 600 }}>เดือน {m.month}/{m.year}</span>
                    <span style={{ color: 'var(--income)' }}>รับ +฿{formatMoney(m.income)}</span>
                    <span style={{ color: 'var(--expense)' }}>จ่าย -฿{formatMoney(m.expense)}</span>
                  </div>
                ))
              )}
            </div>
          </section>
        </div>
      </main>

      {/* Add Transaction Modal */}
      {isModalOpen && (
        <div className="modal-overlay" onClick={() => setIsModalOpen(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3 className="modal-title">บันทึกรายการใหม่</h3>
              <button className="btn-close" onClick={() => setIsModalOpen(false)}>
                <X size={20} />
              </button>
            </div>

            <form onSubmit={handleCreate}>
              {/* Type Switcher */}
              <div className="type-toggle">
                <button
                  type="button"
                  className={`type-toggle-btn ${!formData.isIncome ? 'active expense' : ''}`}
                  onClick={() => setFormData({ ...formData, isIncome: false })}
                >
                  <TrendingDown size={16} />
                  รายจ่าย (Expense)
                </button>
                <button
                  type="button"
                  className={`type-toggle-btn ${formData.isIncome ? 'active income' : ''}`}
                  onClick={() => setFormData({ ...formData, isIncome: true })}
                >
                  <TrendingUp size={16} />
                  รายรับ (Income)
                </button>
              </div>

              {/* Description */}
              <div className="form-group">
                <label className="form-label">รายละเอียดรายการ</label>
                <input
                  type="text"
                  className="form-input"
                  placeholder="เช่น ข้าวกะเพราหมูกรอบ, ซื้อกาแฟ, เงินเดือน"
                  value={formData.description}
                  onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  required
                  autoFocus
                />
              </div>

              {/* Amount */}
              <div className="form-group">
                <label className="form-label">จำนวนเงิน (บาท)</label>
                <input
                  type="number"
                  step="0.01"
                  min="0.01"
                  className="form-input"
                  placeholder="0.00"
                  value={formData.amount}
                  onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                  required
                />
              </div>

              {/* Category (only if Expense) */}
              {!formData.isIncome && (
                <div className="form-group">
                  <label className="form-label">หมวดหมู่</label>
                  <select
                    className="form-select"
                    value={formData.category}
                    onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                  >
                    {categories
                      .filter((c) => c.name !== 'Salary')
                      .map((cat) => (
                        <option key={cat.id} value={cat.name}>
                          {CATEGORY_NAMES[cat.name] || cat.name}
                        </option>
                      ))}
                  </select>
                </div>
              )}

              {/* Date */}
              <div className="form-group">
                <label className="form-label">วันที่</label>
                <input
                  type="date"
                  className="form-input"
                  value={formData.date}
                  onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                />
              </div>

              <div style={{ display: 'flex', gap: '0.75rem', marginTop: '1.75rem' }}>
                <button
                  type="button"
                  className="btn"
                  style={{ flex: 1, background: 'rgba(255,255,255,0.06)', color: 'var(--text-muted)' }}
                  onClick={() => setIsModalOpen(false)}
                >
                  ยกเลิก
                </button>
                <button
                  type="submit"
                  className="btn btn-primary"
                  style={{ flex: 1 }}
                  disabled={submitting}
                >
                  {submitting ? 'กำลังบันทึก...' : 'บันทึกรายการ'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

# MANAGED UBI SYSTEM DESIGN
## Employee Experience Architecture for Purpose-Driven Organizations

---

## EXECUTIVE SUMMARY

The Managed UBI System decouples employee survival from company compliance by:

1. **Fiduciary-Managed Universal Income** — Employees receive a non-conditional income grant managed by a third-party fiduciary
2. **Progressive Asset Estate** — Company contribution grows employee ownership (real estate, securities, dividend streams)
3. **Purpose-Market Interface** — Employees "purchase" purpose by committing labor; company sells purpose by maintaining experience parity with customer design
4. **Real-Time Presence State** — System tracks alignment between employee values and company mission, enabling early intervention
5. **Financial Transparency** — Every employee sees real-time estate growth, asset allocation, and future income projections

This design makes **work optional and purpose mandatory**, inverting the traditional coercion model.

---

## SYSTEM ARCHITECTURE

### High-Level Components

```
┌─────────────────────────────────────────────────────────────┐
│                    MANAGED UBI SYSTEM                        │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌──────────────────┐    ┌──────────────────┐              │
│  │  EMPLOYEE LAYER  │    │  COMPANY LAYER   │              │
│  ├──────────────────┤    ├──────────────────┤              │
│  │ • Profile        │    │ • Purpose Canvas │              │
│  │ • Purpose Lens   │    │ • Presence Index │              │
│  │ • Estate Viewer  │    │ • Commitment Map │              │
│  │ • Time Ledger    │    │ • Revenue Pool   │              │
│  └──────────────────┘    └──────────────────┘              │
│           │                      │                           │
│           └──────────┬───────────┘                           │
│                      │                                        │
│           ┌──────────▼──────────┐                            │
│           │ RECONCILIATION HUB  │                            │
│           ├────────────────────┤                            │
│           │ • Match Making     │                            │
│           │ • Vesting Logic    │                            │
│           │ • Payout Scheduler │                            │
│           └────────────────────┘                            │
│                      │                                        │
│           ┌──────────┴──────────┐                            │
│           │                     │                             │
│     ┌─────▼────────┐     ┌──────▼────────┐                 │
│     │ FIDUCIARY    │     │ ASSET MANAGER │                 │
│     │ SERVICE      │     │               │                 │
│     │ • Grants     │     │ • Allocation  │                 │
│     │ • UBI Ledger │     │ • Rebalance   │                 │
│     │ • Payouts    │     │ • Dividends   │                 │
│     └──────────────┘     └───────────────┘                 │
│                                                               │
└─────────────────────────────────────────────────────────────┘
```

### Architectural Principles

| Principle | Implementation |
|-----------|-----------------|
| **Separation of Concerns** | Fiduciary, Company, and Employee data are logically separate; none controls the others directly |
| **Transparency** | Every employee sees real-time calculations, asset composition, and future projections |
| **Immutability** | All financial transactions create append-only ledgers; no retroactive changes without audit trail |
| **Fault Tolerance** | System continues operating if company fails; fiduciary is independent institution |
| **Presence Tracking** | Non-invasive signals of engagement (project completion, peer feedback, self-reported alignment) |

---

## DATA MODELS

### EMPLOYEE ENTITY

```json
{
  "employee_id": "EMP-2024-0001",
  "legal_name": "Edward Elric",
  "contact": {
    "email": "edward@company.example",
    "phone": "+1-555-0001"
  },
  "hire_date": "2024-01-15",
  "employment_status": "ACTIVE|ON_LEAVE|PAUSED|TERMINATED",
  "role": {
    "title": "Research Lead",
    "department": "Materials Science",
    "reports_to": "EMP-2023-0005"
  },
  "compensation": {
    "ubi_annual_grant": 60000,
    "company_asset_contribution": 50000,
    "vesting_schedule": "CLIFF_2_YEARS|LINEAR_MONTHLY",
    "vesting_start_date": "2024-01-15"
  },
  "fiduciary_account": {
    "fiduciary_id": "FIDC-2024-0001",
    "account_number": "XXXX-XXXX-XXXX-1234",
    "current_balance": 15000.00,
    "asset_portfolio": {
      "liquid_reserves": 8000.00,
      "dividend_stocks": 4500.00,
      "real_estate_fund": 2500.00
    }
  },
  "presence": {
    "purpose_alignment_score": 0.87,
    "project_completion_rate": 0.92,
    "peer_feedback_average": 4.2,
    "last_presence_update": "2024-03-20T14:30:00Z",
    "presence_trend": "STABLE|IMPROVING|DECLINING"
  },
  "metadata": {
    "created_at": "2024-01-15T09:00:00Z",
    "updated_at": "2024-03-20T14:30:00Z",
    "is_deleted": false
  }
}
```

### COMPANY ENTITY

```json
{
  "company_id": "COM-2024-0001",
  "legal_name": "Alchemist Manufacturing Inc.",
  "fiscal_year_start": "2024-01-01",
  "purpose_statement": "Transmute raw materials into precision instruments without equivalent sacrifice",
  "presence_requirements": {
    "minimum_alignment_score": 0.70,
    "review_frequency_days": 90,
    "escalation_threshold": 0.50
  },
  "financial": {
    "annual_revenue": 5000000,
    "ubi_pool_budget": 600000,
    "asset_contribution_pool": 500000,
    "fiduciary_management_fee": 0.01
  },
  "governance": {
    "board_members": ["founder_id", "advisor_id", "external_auditor"],
    "decision_authority": "consensus|ceo|board",
    "transparency_level": "FULL|PARTIAL|MINIMAL"
  },
  "metadata": {
    "created_at": "2024-01-01T00:00:00Z",
    "updated_at": "2024-03-20T14:30:00Z"
  }
}
```

### FIDUCIARY ENTITY

```json
{
  "fiduciary_id": "FIDC-2024-0001",
  "employee_id": "EMP-2024-0001",
  "company_id": "COM-2024-0001",
  "fiduciary_institution": {
    "name": "Third-Party Trust Company",
    "registration": "SEC_REGISTERED|STATE_LICENSED",
    "liability_insurance": true,
    "annual_audit": true
  },
  "account": {
    "account_type": "MANAGED_ESTATE|CUSTODIAL|TRUST",
    "opening_date": "2024-01-15",
    "current_balance": 15000.00,
    "total_contributions": 65000.00,
    "projected_balance_10yr": 450000.00
  },
  "asset_allocation": {
    "target_allocation": {
      "liquid_reserves": 0.30,
      "dividend_growth_stocks": 0.40,
      "real_estate_index_fund": 0.20,
      "emerging_assets": 0.10
    },
    "current_allocation": {
      "liquid_reserves": 0.32,
      "dividend_growth_stocks": 0.38,
      "real_estate_index_fund": 0.22,
      "emerging_assets": 0.08
    },
    "last_rebalance": "2024-03-15"
  },
  "ledger": {
    "entries": [
      {
        "date": "2024-01-15",
        "type": "GRANT|CONTRIBUTION|DIVIDEND|PAYOUT|REBALANCE",
        "amount": 5000.00,
        "source": "company_asset_pool",
        "description": "Monthly asset contribution"
      }
    ]
  },
  "restrictions": {
    "withdrawal_rules": "CLIFF_2_YEARS|AGE_59.5|SEPARATION",
    "rebalance_frequency": "QUARTERLY",
    "advisor_access": "READONLY|NONE"
  }
}
```

### PURPOSE CANVAS (Company)

```json
{
  "canvas_id": "PUR-2024-0001",
  "company_id": "COM-2024-0001",
  "creation_date": "2024-01-01",
  "last_updated": "2024-03-20",
  
  "problem_statement": "Current materials science requires unethical labor practices and environmental degradation",
  "vision": "Manufacturing that honors both innovation and human dignity",
  
  "core_values": [
    {
      "value": "Wholistic Integration",
      "definition": "Employee brings entire self to work without compartmentalization"
    },
    {
      "value": "Informed Consent",
      "definition": "Every employee understands and voluntarily chooses their purpose"
    },
    {
      "value": "Radical Transparency",
      "definition": "All financial and operational data is visible to those affected by it"
    }
  ],
  
  "customer_experience_parity": {
    "customer_experience": {
      "description": "Marketing-engineered psychological reward",
      "mechanisms": ["dopamine_loops", "belonging", "anticipated_delight"]
    },
    "employee_experience_parallel": {
      "description": "Purpose-engineered intrinsic reward",
      "mechanisms": ["autonomy", "mastery", "meaning"]
    },
    "parity_requirement": "Design quality and intentionality must be equal"
  },
  
  "commitment_model": {
    "type": "PURPOSE_MARKET",
    "description": "Employees purchase purpose with labor; company sells purpose by maintaining experience design",
    "implied_covenant": "If company fails to deliver meaningful purpose, employee can exit without financial penalty"
  },
  
  "success_metrics": [
    "Presence Alignment Score ≥ 0.80 across organization",
    "Turnover rate < 15% annually",
    "Employee-recommended purpose match > 85%",
    "Project completion quality rating > 4.5/5"
  ]
}
```

### PRESENCE STATE VECTOR

```json
{
  "presence_state_id": "PRS-2024-0001-03-20",
  "employee_id": "EMP-2024-0001",
  "company_id": "COM-2024-0001",
  "measurement_date": "2024-03-20",
  
  "alignment_signals": {
    "purpose_alignment": {
      "value": 0.87,
      "source": "SELF_REPORT|360_REVIEW|PROJECT_OUTCOMES",
      "trend": "STABLE",
      "confidence": 0.92
    },
    "project_completion": {
      "value": 0.92,
      "completed_projects": 12,
      "abandoned_projects": 1,
      "on_time_delivery_rate": 0.95,
      "quality_rating_avg": 4.6
    },
    "peer_feedback": {
      "value": 4.2,
      "respondents": 8,
      "key_themes": ["integrity", "presence", "vision_alignment"],
      "friction_points": []
    },
    "self_reported_wholeness": {
      "value": 0.88,
      "description": "Employee rates degree to which they bring full self to work",
      "methodology": "QUARTERLY_SURVEY"
    },
    "autonomy_experience": {
      "value": 0.91,
      "description": "Freedom in decision-making about how to accomplish goals",
      "constraint_flags": []
    }
  },
  
  "composite_presence_score": 0.87,
  "risk_level": "GREEN|YELLOW|RED",
  "recommended_action": "MAINTAIN|DISCUSSION|INTERVENTION",
  
  "audit_trail": {
    "created_by": "SYSTEM_AUTOMATED",
    "reviewed_by": "MANAGER_ID",
    "dispute_period_until": "2024-03-27"
  }
}
```

---

## CORE WORKFLOWS

### WORKFLOW 1: EMPLOYEE ONBOARDING & FIDUCIARY SETUP

```
TRIGGER: Offer Accepted / Employment Contract Signed

1. EMPLOYEE REGISTRATION
   └─ Capture legal identity, tax ID, banking info
   └─ Create unique Employee ID
   └─ Set role, department, reporting structure

2. COMPENSATION DESIGN
   └─ Define UBI Annual Grant (e.g., $60,000)
   └─ Define Company Asset Contribution (e.g., $50,000/year)
   └─ Choose vesting schedule (CLIFF_2YRS | LINEAR_MONTHLY)
   └─ Document in Employment Agreement

3. FIDUCIARY ENGAGEMENT
   └─ Company identifies pre-vetted fiduciary institution
   └─ Employee reviews and approves fiduciary terms
   └─ Company signs Fiduciary Agreement (establishes liability)
   └─ Fiduciary creates Managed Estate Account
   └─ First UBI deposit: Month 1 grant amount (1/12 annual)

4. ASSET ALLOCATION CONFIGURATION
   └─ Fiduciary conducts brief risk profile assessment
   └─ Default allocation: 30% Liquid | 40% Dividend Stocks | 20% Real Estate | 10% Emerging
   └─ Employee can customize allocation
   └─ Fiduciary begins monthly asset purchases per allocation

5. PURPOSE ALIGNMENT ASSESSMENT
   └─ Employee completes Purpose Lens questionnaire
   └─ Manager/onboarding team reviews Purpose Canvas with employee
   └─ Initial Presence State Vector created (baseline)
   └─ Employee signs Purpose Covenant (explicit consent)

6. SYSTEM ACCESS
   └─ Employee gains access to:
     ├─ Estate Viewer (real-time balance, asset composition)
     ├─ Presence Dashboard (personal alignment score)
     ├─ Company Purpose Canvas
     ├─10-Year Projection Tool
   └─ First Presence measurement scheduled for Day 90

OUTCOME: Employee has meaningful financial foundation + clear purpose alignment
```

### WORKFLOW 2: MONTHLY ASSET ACCUMULATION & RECONCILIATION

```
TRIGGER: First day of each month (automated)

1. COMPANY REVENUE ASSESSMENT
   └─ Finance pulls prior month's revenue
   └─ Calculate available UBI pool (predefined % of revenue)
   └─ Calculate asset contribution pool (predefined % of profit)

2. ALLOCATION DETERMINATION
   └─ For each active employee:
     ├─ Calculate UBI payout (annual grant / 12)
     ├─ Calculate asset contribution (annual target / 12)
     ├─ Apply any earned adjustments (raises, bonuses)
     ├─ Account for vesting status (if cliff not met, defer contribution)

3. FIDUCIARY DEPOSITS
   └─ Company transfers UBI pool to Fiduciary account
   └─ Company transfers Asset Contribution pool to Fiduciary account
   └─ Fiduciary confirms receipt and begins execution

4. ASSET PURCHASES
   └─ Fiduciary executes monthly asset purchases per allocation:
     ├─ Buys index funds (dividend-growth stocks)
     ├─ Buys real estate ETFs
     ├─ Maintains liquid reserves
     └─ Rebalances quarterly if drift exceeds threshold

5. DIVIDEND CAPTURE
   └─ If dividend-paying assets hit dividend date:
     ├─ Fiduciary captures dividend
     ├─ Reinvests per allocation (or distributes if requested)
     └─ Updates ledger with dividend credit

6. RECONCILIATION & NOTIFICATION
   └─ Fiduciary generates transaction report
   └─ Employee Estate Viewer updates automatically
   └─ Employee receives notification:
     ├─ "Your UBI payment: $5,000"
     ├─ "Your asset contribution this month: $4,167"
     ├─ "Your estate is now worth: $23,456"
     ├─ "Projected 10-year balance: $450,000"

OUTCOME: Employee wealth compounding + transparency + financial hope
```

### WORKFLOW 3: PRESENCE ASSESSMENT & ALIGNMENT CHECK (Quarterly)

```
TRIGGER: Every 90 days from hire date

1. DATA COLLECTION
   └─ Automated systems gather:
     ├─ Project completion metrics
     ├─ Code/work quality metrics
     ├─ Deadline adherence
     └─ Peer collaboration signals

2. SURVEY ADMINISTRATION
   └─ Employee completes self-assessment:
     ├─ Purpose alignment rating (1–10)
     ├─ Wholeness rating (1–10)
     ├─ Autonomy rating (1–10)
     ├─ Any friction points or unmet needs
   └─ Peer review (360): Manager + 5 colleagues
     ├─ Rate presence and engagement
     ├─ Note specific contributions
     ├─ Flag any concerns

3. PRESENCE STATE CALCULATION
   └─ System computes composite score:
     ├─ (Project completion: 35%)
     ├─ + (Self-reported alignment: 25%)
     ├─ + (Peer feedback: 25%)
     ├─ + (Autonomy/wholeness: 15%)
   └─ Result: 0.00–1.00 presence score

4. RISK CLASSIFICATION
   └─ If score ≥ 0.80: GREEN (healthy)
   └─ If score 0.60–0.79: YELLOW (needs discussion)
   └─ If score < 0.60: RED (intervention required)

5. MANAGER REVIEW & DISCUSSION
   └─ Manager reviews presence data with employee
   └─ If GREEN:
     ├─ Celebrate alignment
     ├─ Discuss any growth areas
     ├─ Affirm purpose fit
   └─ If YELLOW:
     ├─ Deep conversation about misalignment
     ├─ Explore role adjustment, purpose pivot, or support needs
     ├─ Set 30-day improvement goals
   └─ If RED:
     ├─ Immediate intervention (next week)
     ├─ Address: Is the company failing to deliver purpose? Or is the employee misaligned?
     ├─ If company at fault: Increase financial support or role redesign
     ├─ If employee at fault: Discuss separation or transition with full estate payout

6. DOCUMENTATION & ESCALATION
   └─ Presence assessment stored in employee record
   └─ If RED or declining trend: Escalate to HR + Leadership
   └─ Company reviews Purpose Canvas (if pattern emerges)

7. SYSTEM UPDATE
   └─ Estate Viewer shows "Presence Trend" graphically
   └─ Employee can see their own trajectory
   └─ Fiduciary alerted to RED status (legal documentation)

OUTCOME: Early detection of misalignment + proactive support + psychological safety
```

### WORKFLOW 4: VESTING & CLIFF EVENTS

```
TRIGGER: Cliff date reached OR anniversary milestones

VESTING SCHEDULE OPTION 1: 2-YEAR CLIFF
└─ At day 1: UBI grant vests immediately (employee can spend anytime)
└─ At month 24: Company asset contributions vest in full
└─ Before cliff: Asset contributions held by fiduciary, withdrawable only on separation

VESTING SCHEDULE OPTION 2: LINEAR MONTHLY
└─ Each month: 1/36th of annual asset contribution vests
└─ By month 36: 100% vesting complete
└─ Employee can withdraw vested amounts anytime

TRIGGER: 2-YEAR CLIFF DATE REACHED

1. ELIGIBILITY CHECK
   └─ System confirms employment status = ACTIVE
   └─ Verify no disputed presence issues

2. ASSET TRANSFER
   └─ Fiduciary marks all company asset contributions as "fully owned"
   └─ Employee gains true legal ownership
   └─ System generates vest confirmation document

3. PSYCHOLOGICAL MILESTONE
   └─ Employee notification:
     ├─ "You've reached your 2-year milestone!"
     ├─ "Your estate is now $87,450 (fully owned by you)"
     ├─ "Dividend income: $350/month"
     ├─ "You could live on your asset income if you chose to leave"
   └─ Manager celebration (explicit recognition)

4. CONTINUED COMMITMENT CHOICE
   └─ System asks: "Do you choose to stay?"
   └─ If NO (separation):
     ├─ Fiduciary disburses full estate to employee
     ├─ Company pays separation benefits (generous, no cliffs)
     ├─ Employee maintains dividend income forever
   └─ If YES (recommitment):
     ├─ Presence reset: New assessment, fresh alignment check
     ├─ Raise discussion: Asset contribution increase or new vesting period?
     ├─ Psychological shift: "You work because you choose to, not because you need to"

OUTCOME: Psychological liberation + real choice + renewed commitment
```

### WORKFLOW 5: SEPARATION & ESTATE PAYOUT

```
TRIGGER: Employee resignation | Termination (cause/no-cause) | Disability | Death

CASE A: VOLUNTARY RESIGNATION (Good Standing, Presence ≥ 0.70)

1. NOTIFICATION & TRANSITION
   └─ Employee submits resignation
   └─ Standard notice period applied (e.g., 2 weeks)
   └─ Manager + HR execute transition plan

2. ESTATE CALCULATION
   └─ Fiduciary calculates total estate value
   └─ Determine vested vs. unvested amounts:
     ├─ If cliff not reached: Unvested contributions forfeited to company
     ├─ If cliff reached or linear: Employee receives 100% of vested
   └─ Generate final accounting statement

3. FINAL PAYOUT
   └─ Fiduciary disburses:
     ├─ All vested assets to employee account
     ├─ Liquid reserves to employee bank account
     ├─ Dividend-paying assets (choice: liquidate or transfer)
   └─ Company issues final paycheck + accrued benefits

4. ONGOING INCOME
   └─ If cliff reached: Employee retains dividend-paying assets
     ├─ Passive income continues forever
     ├─ No cliffs, no vesting — true ownership
   └─ Employee never returns to "zero"

---

CASE B: INVOLUNTARY TERMINATION (No Cause)

1. SEPARATION BENEFIT TRIGGER
   └─ Immediately vest all unvested contributions
   └─ Add severance: 1 month per year of service (minimum 3 months)
   └─ Add garden leave: 1 month paid with benefits maintained

2. ESTATE ACCELERATION
   └─ Fiduciary accelerates all vesting
   └─ Full estate disbursement within 30 days
   └─ Employee receives:
     ├─ All personal assets
     ├─ Severance (cash)
     ├─ Garden leave (continued income)

3. OUTPLACEMENT SUPPORT
   └─ Company funds 6 months of career coaching
   └─ Professional references guaranteed
   └─ Alumni network access maintained

---

CASE C: TERMINATION (For Cause)

1. CAUSE DETERMINATION
   └─ Clear documented violation of core values or ethics
   └─ Following documented warning + improvement period

2. ESTATE TREATMENT
   └─ All vested assets go to employee (never forfeited)
   └─ Unvested assets: forfeited to company
   └─ Final paycheck issued

3. GOOD-FAITH PRINCIPLE
   └─ Even in termination for cause:
     ├─ UBI grant remains (what's been paid out)
     ├─ Vested company contributions remain
     ├─ Only unvested contributions at risk

---

CASE D: DISABILITY | DEATH

1. IMMEDIATE VESTING
   └─ All unvested contributions immediately vest
   └─ Full estate available to employee (or estate)

2. DISABILITY INCOME
   └─ UBI grant continues at full amount
   └─ Asset contributions continue (company pays both)
   └─ Company maintains health insurance + disability insurance

3. DEATH BENEFICIARY
   └─ Estate disbursed per beneficiary designation
   └─ Life insurance (optional company-funded) pays lump sum
   └─ Spouse/dependents can transition to continued income if desired

OUTCOME: Financial safety net + dignity maintained + no "pit" after leaving
```

---

## REAL-TIME DASHBOARDS & USER INTERFACES

### EMPLOYEE ESTATE VIEWER

**Purpose:** Real-time wealth transparency

```
┌──────────────────────────────────────────────────────┐
│          YOUR MANAGED ESTATE (Edward Elric)          │
├──────────────────────────────────────────────────────┤
│                                                       │
│  CURRENT BALANCE                                     │
│  $23,456.78                                          │
│  └─ Up $1,234 this month                             │
│                                                       │
├─────────────────────────────────────────────────────┤
│  ASSET COMPOSITION                                   │
│                                                       │
│  Liquid Reserves:          $7,037      30%           │
│  Dividend Growth Stocks:   $9,382      40%           │
│  Real Estate Index Fund:   $4,691      20%           │
│  Emerging Assets:          $2,345      10%           │
│                                                       │
├─────────────────────────────────────────────────────┤
│  VESTING STATUS                                      │
│                                                       │
│  Cliff 2-Year Milestone: 18 months remaining        │
│  ████████████░░░░ (75% complete)                     │
│                                                       │
│  When you reach 2 years, you'll own $87,450 outright│
│                                                       │
├─────────────────────────────────────────────────────┤
│  10-YEAR PROJECTION                                  │
│                                                       │
│  Current:           $23,457                          │
│  In 5 years:        $145,000 (est.)                  │
│  In 10 years:       $450,000 (est.)                  │
│                                                       │
│  At 10 years: $1,800/month passive income            │
│                                                       │
├─────────────────────────────────────────────────────┤
│  THIS MONTH'S TRANSACTIONS                           │
│                                                       │
│  Mar 1:  UBI Deposit               +$5,000           │
│  Mar 1:  Asset Contribution        +$4,167           │
│  Mar 15: Dividend Capture          +$67              │
│                                                       │
│  [View Full Ledger] [Download Statement]             │
│                                                       │
└──────────────────────────────────────────────────────┘
```

### PRESENCE DASHBOARD

**Purpose:** Self-awareness + early intervention

```
┌──────────────────────────────────────────────────────┐
│           YOUR PRESENCE STATE (Edward Elric)         │
├──────────────────────────────────────────────────────┤
│                                                       │
│  ALIGNMENT SCORE                                     │
│                                                       │
│  0.87 / 1.00  [████████░]  GREEN (Healthy)           │
│                                                       │
│  You are bringing your whole self to work.           │
│  Your purpose and the company's vision align.        │
│                                                       │
├─────────────────────────────────────────────────────┤
│  COMPONENT BREAKDOWN (Last 90 days)                  │
│                                                       │
│  Project Completion:       0.92 (12/13 completed)    │
│  Self-Reported Alignment:  0.88 (Q&A survey)         │
│  Peer Feedback:            4.2/5 (8 responses)       │
│  Autonomy Experience:      0.91 (freedom rating)     │
│                                                       │
├─────────────────────────────────────────────────────┤
│  TREND ANALYSIS (6 months)                           │
│                                                       │
│  0.85 ─╱────────                                     │
│  0.87 ─╱  Stable                                     │
│  0.88 ─┘                                              │
│        └─ Slight upward trend                        │
│                                                       │
├─────────────────────────────────────────────────────┤
│  FRICTION POINTS (If Any)                            │
│                                                       │
│  ✓ None flagged                                      │
│                                                       │
│  [Request Check-In]  [View Full Assessment]          │
│                                                       │
└──────────────────────────────────────────────────────┘
```

### COMPANY PRESENCE INDEX

**Purpose:** Organizational health + early warning system

```
┌────────────────────────────────────────────────────┐
│     ORGANIZATIONAL PRESENCE INDEX                  │
│     Alchemist Manufacturing Inc.                   │
├────────────────────────────────────────────────────┤
│                                                    │
│  OVERALL HEALTH                                    │
│                                                    │
│  0.81 / 1.00  [████████░]  STRONG                 │
│                                                    │
│  23 employees active                               │
│  19 GREEN | 3 YELLOW | 1 RED                       │
│                                                    │
├────────────────────────────────────────────────────┤
│  RED FLAGS (Requires Attention)                    │
│                                                    │
│  🔴 Elena Vasquez (Materials Lab)                  │
│     Presence: 0.58 │ Trend: ↘ DECLINING           │
│     Last score: 0.62 (down from 0.78)              │
│     Indicator: Project delays + low peer feedback  │
│     Recommendation: Immediate conversation         │
│                                                    │
├────────────────────────────────────────────────────┤
│  YELLOW FLAGS (Watch)                              │
│                                                    │
│  🟡 Marcus Chen (Finance)                          │
│     Presence: 0.67 │ Trend: ↘ SLIGHT DECLINE     │
│     Notes: Team friction (unresolved)              │
│     Action: Discussion scheduled for next week     │
│                                                    │
├────────────────────────────────────────────────────┤
│  DEPARTMENT BREAKDOWN                              │
│                                                    │
│  Engineering:       0.84 (7 employees)             │
│  Materials Lab:     0.78 (5 employees)             │
│  Finance/Admin:     0.79 (4 employees)             │
│  Leadership:        0.91 (2 employees)             │
│                                                    │
├────────────────────────────────────────────────────┤
│  COMPANY HEALTH METRICS                            │
│                                                    │
│  Voluntary Turnover (YTD):    4% (target: <10%)   │
│  Avg. Tenure:                 3.2 years            │
│  Purpose Alignment Match:     0.81 avg.            │
│  Employee Recommendation Rate: 87%                 │
│                                                    │
│  [View Detailed Report]  [Manage Interventions]    │
│                                                    │
└────────────────────────────────────────────────────┘
```

---

## API ENDPOINTS (RESTful Design)

### EMPLOYEE ENDPOINTS

```
GET /api/employees/{employee_id}
└─ Retrieve employee profile

POST /api/employees
└─ Create new employee (onboarding)

GET /api/employees/{employee_id}/estate
└─ Real-time estate balance + composition

GET /api/employees/{employee_id}/presence
└─ Current presence state + trend

POST /api/employees/{employee_id}/presence/update
└─ Submit self-reported presence data (survey)

GET /api/employees/{employee_id}/ledger
└─ Full transaction history

GET /api/employees/{employee_id}/projections
└─ 5-year, 10-year wealth forecast

POST /api/employees/{employee_id}/separation
└─ Initiate separation workflow
```

### FIDUCIARY ENDPOINTS

```
POST /api/fiduciary/accounts
└─ Create managed estate account

GET /api/fiduciary/accounts/{account_id}/balance
└─ Current balance + composition

POST /api/fiduciary/transactions
└─ Record deposit, purchase, dividend, etc.

GET /api/fiduciary/accounts/{account_id}/transactions
└─ Account ledger

POST /api/fiduciary/rebalance
└─ Trigger quarterly rebalancing

GET /api/fiduciary/accounts/{account_id}/projections
└─ Calculate future value scenarios
```

### COMPANY ENDPOINTS

```
GET /api/companies/{company_id}/presence-index
└─ Overall organizational health

GET /api/companies/{company_id}/purpose-canvas
└─ Company's current purpose statement + values

PUT /api/companies/{company_id}/purpose-canvas
└─ Update company purpose (requires board consensus)

GET /api/companies/{company_id}/employees
└─ All employee presence states (aggregated, anonymized if needed)

POST /api/companies/{company_id}/monthly-reconciliation
└─ Trigger monthly UBI/asset allocation cycle

GET /api/companies/{company_id}/financial-impact
└─ Cost analysis: UBI spend vs. productivity gains
```

### ADMIN / LEADERSHIP ENDPOINTS

```
GET /api/admin/red-flags
└─ All employees below presence threshold

POST /api/admin/interventions/{employee_id}
└─ Log intervention (conversation, support, role change)

GET /api/admin/turnover-analysis
└─ Historical departure data + exit interview themes

POST /api/admin/audit-trail
└─ Generate compliance report for fiduciary review
```

---

## SAFETY GUARDS & COMPLIANCE

### GUARD 1: FIDUCIARY INDEPENDENCE

**Principle:** No single entity controls the employee's wealth

```
├─ Fiduciary Account:
│  └─ Held by third-party registered institution
│  └─ Company cannot access without employee authorization
│  └─ Annual independent audit required
│  └─ Bonded liability insurance (minimum $1M)
│
└─ Separation:
   └─ If company fails, fiduciary continues managing account
   └─ Employee retains all vested assets
   └─ Unvested contributions may be subject to insolvency law
```

### GUARD 2: INFORMED CONSENT DOCUMENTATION

**Principle:** Employee explicitly agrees to the system (not tricked into it)

```
├─ Onboarding Ceremony:
│  └─ Employee signs Purpose Covenant
│  └─ Employee acknowledges UBI terms
│  └─ Employee reviews asset allocation with fiduciary
│  └─ Employee can customize or decline (with consequences)
│
└─ Annual Affirmation:
   └─ Each year, employee reaffirms commitment
   └─ OR triggers reassessment / exit conversation
```

### GUARD 3: PRESENCE DISPUTE RESOLUTION

**Principle:** Presence scores are not unilateral judgment; employee can dispute

```
├─ Assessment Dispute Period: 7 days after measurement
│  └─ Employee can submit rebuttal
│  └─ Manager + HR review dispute
│  └─ Third party (ombudsperson) arbitrates if needed
│
└─ If Score Challenged:
   └─ System holds action pending resolution
   └─ Employee retains all payments during dispute
   └─ Burden of proof on company to substantiate score
```

### GUARD 4: MINIMUM ASSET ALLOCATION

**Principle:** Company cannot force all assets into risky products

```
├─ Liquid Reserves: Minimum 25% (accessible emergency funds)
├─ Dividend Stocks: Maximum 50% (stable income)
├─ Real Estate: 15–30% (inflation hedge)
└─ Emerging Assets: Maximum 15% (speculation cap)
```

### GUARD 5: SEPARATION WITHOUT PENALTY

**Principle:** Leaving the company is never financially punitive

```
├─ Voluntary Resignation: Take all vested + receive severance if involuntary
├─ No Cliff Clawback: If 2-year cliff passed, assets are forever yours
├─ Death/Disability: Immediate 100% vesting + estate disbursement
└─ For Cause Termination: Only unvested assets at risk (vested always yours)
```

### GUARD 6: TRANSPARENCY AUDIT TRAIL

**Principle:** Every decision is logged and auditable

```
├─ Immutable Ledger: All transactions append-only
├─ Decision Log: Who made what change and why
├─ Fiduciary Reports: Monthly to company + employee
└─ Annual Audit: Third-party verified (SOC 2 compliance)
```

### GUARD 7: PROXY PROTECTION (Privacy)

**Principle:** Presence measurement doesn't become surveillance

```
├─ Prohibited Monitoring:
│  ├─ No keystroke logging
│  ├─ No webcam surveillance
│  ├─ No location tracking
│  └─ No social media scraping
│
├─ Allowed Signals:
│  ├─ Project deliverables (submitted work)
│  ├─ Peer feedback (voluntary, aggregated)
│  ├─ Self-reported survey (optional)
│  └─ Deadline adherence (factual data)
│
└─ Principle: Measure outputs, not behaviors
```

---

## IMPLEMENTATION ROADMAP

### PHASE 1: Foundation (Months 1–3)

**Deliverables:**
- Core data models (Employee, Company, Fiduciary, Presence State)
- Fiduciary account integration (API to financial institution)
- Employee Estate Viewer (basic balance display)
- Onboarding workflow (forms + document generation)

**Testing:** Pilot with 5–10 employees (internal team)

### PHASE 2: Intelligence (Months 4–6)

**Deliverables:**
- Presence measurement system (survey + peer feedback)
- Presence Dashboard (visualization of alignment)
- Company Presence Index (organizational health)
- Automated alerting (RED flag detection)

**Testing:** Scale to 20–30 employees; refine measurement accuracy

### PHASE 3: Autonomy & Intelligence (Months 7–9)

**Deliverables:**
- Projections engine (5-year, 10-year forecasts)
- Rebalancing automation (quarterly + on-demand)
- Separation workflow (voluntary, involuntary, disability)
- Audit trail + compliance reporting

**Testing:** Full production rollout to all employees

### PHASE 4: Optimization (Months 10–12)

**Deliverables:**
- Machine learning for asset allocation (risk profiling)
- Predictive presence indicators (churn prediction)
- Purpose Canvas evolution engine (company improvement feedback loop)
- Integration with HR systems (HRIS sync)

**Testing:** Measure outcomes: turnover, productivity, asset growth, presence stability

---

## RISK MITIGATION

| Risk | Mitigation |
|------|-----------|
| **Fiduciary Insolvency** | Bonded insurance + SIPC coverage if securities broker |
| **Company Insolvency (owed UBI)** | Cliff vesting protects employee; unvested at risk |
| **Market Downturn (asset values drop)** | Presence system is *independent* of market performance |
| **Fraudulent Presence Scoring** | Dispute resolution + independent audit |
| **Employee Gaming System** | Peer feedback + project completion metrics cross-validate |
| **Manager Bias** | Automated systems + 360 review + ombudsperson arbitration |
| **Tax Complexity** | Consult IRS on UBI classification; likely gift tax implications |
| **Regulatory Scrutiny** | Align with ERISA if becomes retirement instrument; possibly DOL exemption |

---

## SUCCESS METRICS (Year 1)

| Metric | Target | Rationale |
|--------|--------|-----------|
| **Voluntary Turnover** | < 10% | Meaningful work retention |
| **Presence Alignment Avg** | 0.75+ | Most employees wholistically engaged |
| **RED Flags Resolved** | 90%+ | Early intervention effective |
| **Employee Recommendation Rate** | 85%+ | Would suggest company to others |
| **Asset Growth (Vested)** | 15% YoY | Compound wealth building |
| **Project Quality (avg rating)** | 4.3+ / 5 | Purpose-driven work high quality |
| **Manager-Employee Trust** | 4.2+ / 5 (survey) | Presence ≠ surveillance |

---

## CONCLUSION

The Managed UBI System inverts the traditional employee-company relationship from **coercion via survival** to **collaboration around purpose**. By:

1. Securing the employee's survival (UBI + fiduciary independence)
2. Building progressive wealth (estate accumulation)
3. Measuring alignment (not compliance)
4. Designing experience with intention (not as afterthought)

...the organization attracts and retains employees who are **wholistically present** — bringing their full selves, intellect, creativity, and integrity to the work.

Edward Elric's mature self doesn't serve the Military because he has to. He serves because the work means something to him. That's the shift.

The system supports both the employee (financial liberation + purpose alignment) and the company (dramatically higher human potential + reduced friction from coercion).

---

**Document Version:** 1.0  
**Last Updated:** March 20, 2024  
**Status:** Ready for Technical Specification Phase

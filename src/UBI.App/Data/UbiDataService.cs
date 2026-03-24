using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace UBI.App.Data;

public sealed class UbiDataService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly object _gate = new();
    private readonly string _dataFilePath;
    private UbiState _state;

    public UbiDataService(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
        _state = LoadState();
    }

    public UbiWorkspace Workspace
    {
        get
        {
            lock (_gate)
            {
                return BuildWorkspace(_state);
            }
        }
    }

    public CompanyProfile GetCompany()
    {
        lock (_gate)
        {
            return _state.Company with { };
        }
    }

    public IReadOnlyList<EmployeeProfile> GetEmployees()
    {
        lock (_gate)
        {
            return _state.Employees
                .OrderByDescending(employee => employee.HireDate)
                .ToList();
        }
    }

    public EmployeeProfile? GetEmployee(string employeeId)
    {
        lock (_gate)
        {
            return _state.Employees.FirstOrDefault(employee => employee.EmployeeId == employeeId);
        }
    }

    public IReadOnlyList<InterventionRecord> GetInterventions()
    {
        lock (_gate)
        {
            return _state.Interventions
                .OrderByDescending(intervention => intervention.CreatedAtUtc)
                .ToList();
        }
    }

    public EmployeeProfile AddEmployee(NewEmployeeRequest request)
    {
        lock (_gate)
        {
            if (string.IsNullOrWhiteSpace(request.LegalName))
            {
                throw new InvalidOperationException("Legal name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new InvalidOperationException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(request.RoleTitle))
            {
                throw new InvalidOperationException("Role title is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Department))
            {
                throw new InvalidOperationException("Department is required.");
            }

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            if (_state.Employees.Any(employee => string.Equals(employee.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"An employee with email '{request.Email}' already exists.");
            }

            var employeeIndex = _state.Employees.Count + 1;
            var employeeId = $"EMP-{DateTime.UtcNow:yyyy}-{employeeIndex:0000}";
            var hireDate = DateOnly.FromDateTime(request.HireDate);
            var monthlyUbi = request.UbiAnnualGrant / 12m;
            var monthlyContribution = request.AssetContributionAnnual / 12m;
            var currentBalance = Math.Round((monthlyUbi + monthlyContribution) * 3.8m, 2);
            var currentOutrightOwnership = Math.Round(request.AssetContributionAnnual * 1.75m, 2);

            var employee = new EmployeeProfile
            {
                EmployeeId = employeeId,
                LegalName = request.LegalName.Trim(),
                Email = normalizedEmail,
                Department = request.Department.Trim(),
                RoleTitle = request.RoleTitle.Trim(),
                EmploymentStatus = "ACTIVE",
                HireDate = hireDate,
                UbiAnnualGrant = request.UbiAnnualGrant,
                AssetContributionAnnual = request.AssetContributionAnnual,
                VestingSchedule = "CLIFF_2_YEARS",
                VestingStartDate = hireDate,
                Estate = new EstateProfile
                {
                    CurrentBalance = currentBalance,
                    MonthlyChange = Math.Round(monthlyUbi + monthlyContribution, 2),
                    PassiveIncomeMonthly = Math.Round(currentBalance * 0.075m, 0),
                    CurrentOutrightOwnership = currentOutrightOwnership,
                    CliffMonthsRemaining = 24,
                    VestingPercent = 0,
                    AssetComposition =
                    [
                        new AssetAllocationItem("Liquid reserves", Math.Round(currentBalance * 0.30m, 2), 30, "green"),
                        new AssetAllocationItem("Dividend growth stocks", Math.Round(currentBalance * 0.40m, 2), 40, "blue"),
                        new AssetAllocationItem("Real estate index fund", Math.Round(currentBalance * 0.20m, 2), 20, "gold"),
                        new AssetAllocationItem("Emerging assets", Math.Round(currentBalance * 0.10m, 2), 10, "purple")
                    ],
                    Projections =
                    [
                        new ProjectionPoint("Current estate", currentBalance, "Seeded from the onboarding workflow"),
                        new ProjectionPoint("5-year forecast", Math.Round(currentBalance + request.AssetContributionAnnual * 4.0m, 0), "Assumes current contribution policy"),
                        new ProjectionPoint("10-year forecast", Math.Round(currentBalance + request.AssetContributionAnnual * 9.5m, 0), "Long-term estate growth target")
                    ],
                    Transactions =
                    [
                        new LedgerEntry(FormatDateLabel(DateTime.UtcNow), "Initial managed estate setup", Math.Round(currentBalance, 0), "green")
                    ]
                },
                Presence = new PresenceProfile
                {
                    AlignmentScore = 0.78,
                    ProjectCompletionRate = 0.80,
                    SelfReportedAlignment = 0.79,
                    PeerFeedbackAverage = 4.0,
                    AutonomyExperience = 0.76,
                    Trend = "Establishing baseline",
                    TrendHistory =
                    [
                        new TrendPoint("Start", 0.78, "Onboarding baseline")
                    ]
                }
            };

            _state.Employees.Add(employee);
            _state.Activity.Add(new ActivityRecord
            {
                OccurredAtUtc = DateTime.UtcNow,
                Title = $"Onboarded {employee.LegalName}",
                Description = $"Created {employee.RoleTitle} profile in {employee.Department} with a managed estate baseline.",
                Tone = "green"
            });

            SaveStateUnsafe();
            return employee;
        }
    }

    public InterventionRecord AddIntervention(NewInterventionRequest request)
    {
        lock (_gate)
        {
            if (string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                throw new InvalidOperationException("An employee must be selected.");
            }

            if (string.IsNullOrWhiteSpace(request.Summary))
            {
                throw new InvalidOperationException("Intervention summary is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Recommendation))
            {
                throw new InvalidOperationException("A recommendation is required.");
            }

            var employee = _state.Employees.FirstOrDefault(item => item.EmployeeId == request.EmployeeId)
                ?? throw new InvalidOperationException("The selected employee could not be found.");

            var intervention = new InterventionRecord
            {
                InterventionId = $"INT-{DateTime.UtcNow:yyyyMMddHHmmss}",
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.LegalName,
                Owner = request.Owner.Trim(),
                Severity = request.Severity,
                Status = "OPEN",
                Summary = request.Summary.Trim(),
                Recommendation = request.Recommendation.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };

            _state.Interventions.Add(intervention);
            _state.Activity.Add(new ActivityRecord
            {
                OccurredAtUtc = intervention.CreatedAtUtc,
                Title = $"Support intervention for {employee.LegalName}",
                Description = intervention.Summary,
                Tone = SeverityTone(intervention.Severity)
            });

            employee.Presence.AlignmentScore = Math.Min(employee.Presence.AlignmentScore, request.Severity switch
            {
                "RED" => 0.58,
                "YELLOW" => 0.67,
                _ => employee.Presence.AlignmentScore
            });
            employee.Presence.Trend = request.Severity switch
            {
                "RED" => "Declining",
                "YELLOW" => "Slight decline",
                _ => employee.Presence.Trend
            };
            employee.Presence.TrendHistory.Add(new TrendPoint(
                FormatMonthLabel(intervention.CreatedAtUtc),
                employee.Presence.AlignmentScore,
                request.Severity == "RED" ? "Intervention required" : "Support watch"));

            SaveStateUnsafe();
            return intervention;
        }
    }

    public CompanyProfile UpdatePurposeStatement(UpdateCompanyPurposeRequest request)
    {
        lock (_gate)
        {
            if (string.IsNullOrWhiteSpace(request.PurposeStatement))
            {
                throw new InvalidOperationException("Purpose statement cannot be empty.");
            }

            _state.Company = _state.Company with
            {
                PurposeStatement = request.PurposeStatement.Trim()
            };
            _state.Activity.Add(new ActivityRecord
            {
                OccurredAtUtc = DateTime.UtcNow,
                Title = "Purpose statement updated",
                Description = "Leadership refined the mission and purpose-market promise.",
                Tone = "blue"
            });

            SaveStateUnsafe();
            return _state.Company;
        }
    }

    private UbiState LoadState()
    {
        if (File.Exists(_dataFilePath))
        {
            var existing = JsonSerializer.Deserialize<UbiState>(File.ReadAllText(_dataFilePath), JsonOptions);
            if (existing is not null)
            {
                return existing;
            }
        }

        var seededState = BuildSeedState();
        Directory.CreateDirectory(Path.GetDirectoryName(_dataFilePath)!);
        File.WriteAllText(_dataFilePath, JsonSerializer.Serialize(seededState, JsonOptions));
        return seededState;
    }

    private void SaveStateUnsafe()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_dataFilePath)!);
        File.WriteAllText(_dataFilePath, JsonSerializer.Serialize(_state, JsonOptions));
    }

    private static UbiWorkspace BuildWorkspace(UbiState state)
    {
        var activeEmployees = state.Employees
            .Where(employee => employee.EmploymentStatus == "ACTIVE")
            .ToList();

        var featuredEmployee = activeEmployees
            .OrderByDescending(employee => employee.Estate.CurrentBalance)
            .FirstOrDefault()
            ?? state.Employees.First();

        var averageHealth = activeEmployees.Count == 0
            ? 0
            : activeEmployees.Average(employee => employee.Presence.AlignmentScore);
        var greenCount = activeEmployees.Count(employee => employee.Presence.AlignmentScore >= 0.75);
        var yellowCount = activeEmployees.Count(employee => employee.Presence.AlignmentScore >= 0.60 && employee.Presence.AlignmentScore < 0.75);
        var redCount = activeEmployees.Count(employee => employee.Presence.AlignmentScore < 0.60);
        var openInterventions = state.Interventions.Count(intervention => intervention.Status == "OPEN");
        var monthlyFlow = activeEmployees.Sum(employee => employee.UbiAnnualGrant + employee.AssetContributionAnnual) / 12m;

        var departmentScores = activeEmployees
            .GroupBy(employee => employee.Department)
            .Select(group => new DepartmentScore(group.Key, group.Average(item => item.Presence.AlignmentScore), group.Count()))
            .OrderByDescending(item => item.Score)
            .ToList();

        var flagEmployees = state.Interventions
            .OrderByDescending(intervention => SeverityRank(intervention.Severity))
            .ThenByDescending(intervention => intervention.CreatedAtUtc)
            .Take(3)
            .Select(intervention =>
            {
                var employee = state.Employees.First(item => item.EmployeeId == intervention.EmployeeId);
                return new FlaggedEmployee(
                    employee.LegalName,
                    employee.Department,
                    employee.Presence.AlignmentScore,
                    employee.Presence.Trend,
                    intervention.Summary,
                    intervention.Recommendation,
                    SeverityTone(intervention.Severity));
            })
            .ToList();

        if (flagEmployees.Count == 0)
        {
            flagEmployees = activeEmployees
                .OrderBy(employee => employee.Presence.AlignmentScore)
                .Take(2)
                .Select(employee => new FlaggedEmployee(
                    employee.LegalName,
                    employee.Department,
                    employee.Presence.AlignmentScore,
                    employee.Presence.Trend,
                    "No formal intervention logged yet; monitor this employee closely.",
                    "Review workload, mission fit, and support needs.",
                    employee.Presence.AlignmentScore < 0.60 ? "red" : "gold"))
                .ToList();
        }

        return new UbiWorkspace(
            CompanyName: state.Company.LegalName,
            Tagline: state.Company.Tagline,
            PurposeStatement: state.Company.PurposeStatement,
            ActiveEmployees: activeEmployees.Count,
            OpenInterventions: openInterventions,
            MonthlyFlow: Math.Round(monthlyFlow, 0),
            Readiness: "Local MVP with persisted workflows",
            ExperiencePillars:
            [
                new Pillar("Transparent estates", "Employees can inspect balances, vesting, asset mix, and monthly ledger movement from live local state.", "green"),
                new Pillar("Presence before burnout", "Leadership can log interventions and track alignment changes before disengagement turns into attrition.", "blue"),
                new Pillar("Operational guardrails", "Company purpose updates and support work are saved with a durable local activity trail.", "gold")
            ],
            WorkflowStages:
            [
                new WorkflowStage("Onboarding", $"{state.Employees.Count} employee records in the system and ready for managed-estate setup.", "Live"),
                new WorkflowStage("Monthly reconciliation", "Ledger and projection surfaces are present; automated reconciliation remains a next build step.", "Next"),
                new WorkflowStage("Quarterly presence review", $"{openInterventions} active support actions are flowing into the leadership view.", openInterventions > 0 ? "Live" : "Ready"),
                new WorkflowStage("Store release readiness", "Native Flatpak wrapper installed locally; release polish and public submission remain.", "In progress")
            ],
            Guardrails:
            [
                new Guardrail("Fiduciary independence", "Financial UI remains explanatory and auditable; no hidden state changes occur outside the persisted model."),
                new Guardrail("Informed consent trail", "Onboarding and mission updates generate saved activity entries for later audit."),
                new Guardrail("Separation without penalty", "The data model remains compatible with humane separation workflows instead of punitive clawbacks."),
                new Guardrail("Transparent audit log", "Recent activity is derived from persisted onboarding, intervention, and purpose-update events.")
            ],
            RecentActivity: state.Activity
                .OrderByDescending(item => item.OccurredAtUtc)
                .Take(4)
                .Select(item => new ActivityItem(FormatDateLabel(item.OccurredAtUtc), item.Title, item.Description, item.Tone))
                .ToList(),
            Estate: BuildEstateDashboard(featuredEmployee),
            Presence: BuildPresenceDashboard(featuredEmployee),
            Leadership: new LeadershipDashboard(
                OverallHealth: averageHealth,
                HealthLabel: averageHealth >= 0.80 ? "Strong" : averageHealth >= 0.65 ? "Watch" : "Fragile",
                GreenCount: greenCount,
                YellowCount: yellowCount,
                RedCount: redCount,
                AvgTenureYears: activeEmployees.Count == 0 ? 0 : activeEmployees.Average(employee => (DateTime.UtcNow.Date - employee.HireDate.ToDateTime(TimeOnly.MinValue)).TotalDays / 365d),
                RecommendationRate: activeEmployees.Count == 0 ? 0 : Math.Clamp(averageHealth + 0.06, 0, 1),
                DepartmentScores: departmentScores,
                Flags: flagEmployees,
                HealthMetrics:
                [
                    new HealthMetric("Voluntary turnover (YTD)", "0%", "No departures are modeled yet in the local MVP"),
                    new HealthMetric("Purpose alignment match", averageHealth.ToString("0.00", CultureInfo.InvariantCulture), "Average composite presence score"),
                    new HealthMetric("Average estate growth", activeEmployees.Count == 0 ? "$0" : activeEmployees.Average(employee => employee.Estate.MonthlyChange).ToString("C0"), "Monthly employee balance delta"),
                    new HealthMetric("Open support actions", openInterventions.ToString(CultureInfo.InvariantCulture), "Interventions tracked in the operations workflow")
                ],
                InterventionPlaybook:
                [
                    new PlaybookAction("Immediate support", "Meet the employee quickly, clarify the friction, and document concrete support actions."),
                    new PlaybookAction("Recheck mission fit", "Confirm role expectations still align with the company purpose statement and autonomy promise."),
                    new PlaybookAction("Track recovery", "Log the action and watch the next dashboard cycle instead of relying on memory.")
                ]));
    }

    private static EmployeeEstateDashboard BuildEstateDashboard(EmployeeProfile employee)
    {
        return new EmployeeEstateDashboard(
            employee.LegalName,
            employee.Estate.CurrentBalance,
            employee.Estate.MonthlyChange,
            employee.Estate.PassiveIncomeMonthly,
            employee.Estate.CurrentOutrightOwnership,
            employee.Estate.CliffMonthsRemaining,
            employee.Estate.VestingPercent,
            employee.Estate.AssetComposition,
            employee.Estate.Projections,
            employee.Estate.Transactions);
    }

    private static EmployeePresenceDashboard BuildPresenceDashboard(EmployeeProfile employee)
    {
        return new EmployeePresenceDashboard(
            employee.LegalName,
            employee.Presence.AlignmentScore,
            employee.Presence.AlignmentScore >= 0.75 ? "Healthy" : employee.Presence.AlignmentScore >= 0.60 ? "Watch" : "At risk",
            employee.Presence.AlignmentScore >= 0.75 ? "green" : employee.Presence.AlignmentScore >= 0.60 ? "gold" : "red",
            90,
            employee.Presence.AlignmentScore >= 0.75
                ? "Alignment is healthy and the employee still appears meaningfully connected to the company's mission."
                : "This employee is showing enough friction that leadership should actively support role fit, workload, and autonomy.",
            [
                new ScoreBreakdownItem("Project completion", employee.Presence.ProjectCompletionRate, "Delivery confidence over the current review window"),
                new ScoreBreakdownItem("Self-reported alignment", employee.Presence.SelfReportedAlignment, "Employee-reported connection to the mission"),
                new ScoreBreakdownItem("Peer feedback", employee.Presence.PeerFeedbackAverage / 5.0, $"{employee.Presence.PeerFeedbackAverage:0.0} / 5 peer average"),
                new ScoreBreakdownItem("Autonomy experience", employee.Presence.AutonomyExperience, "Perceived freedom and trust within the role")
            ],
            employee.Presence.TrendHistory,
            [
                new FrictionPoint(
                    employee.Presence.Trend,
                    employee.Presence.AlignmentScore >= 0.75
                        ? "No acute friction signals are currently open for this employee."
                        : "Support work is recommended to prevent disengagement from becoming turnover.",
                    employee.Presence.AlignmentScore >= 0.75 ? "green" : "gold"),
                new FrictionPoint("Next suggested action", "Use the operations page to log a support intervention or adjust the mission statement as needed.", "blue")
            ]);
    }

    private static int SeverityRank(string severity) => severity switch
    {
        "RED" => 3,
        "YELLOW" => 2,
        _ => 1
    };

    private static string SeverityTone(string severity) => severity switch
    {
        "RED" => "red",
        "YELLOW" => "gold",
        _ => "blue"
    };

    private static string FormatDateLabel(DateTime value) => value.ToString("MMM dd", CultureInfo.InvariantCulture);

    private static string FormatMonthLabel(DateTime value) => value.ToString("MMM", CultureInfo.InvariantCulture);

    private static UbiState BuildSeedState()
    {
        return new UbiState
        {
            Company = new CompanyProfile
            {
                CompanyId = "COM-2026-0001",
                LegalName = "Alchemist Manufacturing Inc.",
                Tagline = "Employee experience architecture for purpose-driven organizations",
                PurposeStatement = "Transmute raw materials into precision instruments without equivalent sacrifice."
            },
            Employees =
            [
                SeedEmployee("EMP-2026-0001", "Edward Elric", "edward@company.example", "Engineering", "Research Lead", 0.87, 4.2, 23456.78m, 1234m, 18, 75),
                SeedEmployee("EMP-2026-0002", "Elena Vasquez", "elena@company.example", "Materials Lab", "Materials Specialist", 0.58, 3.1, 18440m, 930m, 10, 58),
                SeedEmployee("EMP-2026-0003", "Marcus Chen", "marcus@company.example", "Finance / Admin", "Finance Analyst", 0.67, 3.7, 20540m, 1010m, 14, 63),
                SeedEmployee("EMP-2026-0004", "Winry Rockbell", "winry@company.example", "Engineering", "Operations Engineer", 0.91, 4.6, 26240m, 1320m, 9, 82),
                SeedEmployee("EMP-2026-0005", "Riza Hawkeye", "riza@company.example", "Leadership", "Chief of Staff", 0.89, 4.4, 28100m, 1405m, 6, 88)
            ],
            Interventions =
            [
                new InterventionRecord
                {
                    InterventionId = "INT-20260318103000",
                    EmployeeId = "EMP-2026-0002",
                    EmployeeName = "Elena Vasquez",
                    Owner = "Leadership Team",
                    Severity = "RED",
                    Status = "OPEN",
                    Summary = "Project delays and low peer feedback require immediate support.",
                    Recommendation = "Immediate conversation, role-fit review, and a workload reset within 48 hours.",
                    CreatedAtUtc = new DateTime(2026, 3, 18, 10, 30, 0, DateTimeKind.Utc)
                },
                new InterventionRecord
                {
                    InterventionId = "INT-20260320150000",
                    EmployeeId = "EMP-2026-0003",
                    EmployeeName = "Marcus Chen",
                    Owner = "Leadership Team",
                    Severity = "YELLOW",
                    Status = "OPEN",
                    Summary = "Team friction persists and mission fit should be rechecked.",
                    Recommendation = "Facilitated discussion next week plus a follow-up on role clarity.",
                    CreatedAtUtc = new DateTime(2026, 3, 20, 15, 00, 0, DateTimeKind.Utc)
                }
            ],
            Activity =
            [
                new ActivityRecord
                {
                    OccurredAtUtc = new DateTime(2026, 3, 1, 13, 0, 0, DateTimeKind.Utc),
                    Title = "Monthly UBI grant posted",
                    Description = "Managed-estate balances were refreshed for all active employees.",
                    Tone = "green"
                },
                new ActivityRecord
                {
                    OccurredAtUtc = new DateTime(2026, 3, 6, 16, 15, 0, DateTimeKind.Utc),
                    Title = "Purpose canvas reviewed",
                    Description = "Leadership updated the purpose-market promise for the current quarter.",
                    Tone = "blue"
                },
                new ActivityRecord
                {
                    OccurredAtUtc = new DateTime(2026, 3, 18, 10, 30, 0, DateTimeKind.Utc),
                    Title = "Support conversation scheduled",
                    Description = "Materials Lab decline signal escalated to a human follow-up.",
                    Tone = "red"
                }
            ]
        };
    }

    private static EmployeeProfile SeedEmployee(
        string id,
        string name,
        string email,
        string department,
        string role,
        double alignment,
        double peerFeedback,
        decimal balance,
        decimal monthlyChange,
        int cliffMonthsRemaining,
        int vestingPercent)
    {
        return new EmployeeProfile
        {
            EmployeeId = id,
            LegalName = name,
            Email = email,
            Department = department,
            RoleTitle = role,
            EmploymentStatus = "ACTIVE",
            HireDate = new DateOnly(2024, 1, 15),
            UbiAnnualGrant = 60_000m,
            AssetContributionAnnual = 50_000m,
            VestingSchedule = "CLIFF_2_YEARS",
            VestingStartDate = new DateOnly(2024, 1, 15),
            Estate = new EstateProfile
            {
                CurrentBalance = balance,
                MonthlyChange = monthlyChange,
                PassiveIncomeMonthly = Math.Round(balance * 0.076m, 0),
                CurrentOutrightOwnership = Math.Round(balance * 3.73m, 0),
                CliffMonthsRemaining = cliffMonthsRemaining,
                VestingPercent = vestingPercent,
                AssetComposition =
                [
                    new AssetAllocationItem("Liquid reserves", Math.Round(balance * 0.30m, 2), 30, "green"),
                    new AssetAllocationItem("Dividend growth stocks", Math.Round(balance * 0.40m, 2), 40, "blue"),
                    new AssetAllocationItem("Real estate index fund", Math.Round(balance * 0.20m, 2), 20, "gold"),
                    new AssetAllocationItem("Emerging assets", Math.Round(balance * 0.10m, 2), 10, "purple")
                ],
                Projections =
                [
                    new ProjectionPoint("Current estate", Math.Round(balance, 0), "Live managed balance"),
                    new ProjectionPoint("5-year forecast", Math.Round(balance * 6.2m, 0), "Current policy estimate"),
                    new ProjectionPoint("10-year forecast", Math.Round(balance * 19.2m, 0), "Long-range managed-estate target")
                ],
                Transactions =
                [
                    new LedgerEntry("Mar 01", "UBI deposit", 5_000m, "green"),
                    new LedgerEntry("Mar 01", "Asset contribution", 4_167m, "blue"),
                    new LedgerEntry("Mar 15", "Dividend capture", 67m, "gold")
                ]
            },
            Presence = new PresenceProfile
            {
                AlignmentScore = alignment,
                ProjectCompletionRate = Math.Clamp(alignment + 0.05, 0, 1),
                SelfReportedAlignment = Math.Clamp(alignment + 0.01, 0, 1),
                PeerFeedbackAverage = peerFeedback,
                AutonomyExperience = Math.Clamp(alignment + 0.04, 0, 1),
                Trend = alignment >= 0.75 ? "Stable" : alignment >= 0.60 ? "Slight decline" : "Declining",
                TrendHistory =
                [
                    new TrendPoint("Oct", Math.Clamp(alignment - 0.03, 0, 1), "Baseline reset"),
                    new TrendPoint("Nov", Math.Clamp(alignment - 0.02, 0, 1), "Signal review"),
                    new TrendPoint("Dec", Math.Clamp(alignment - 0.01, 0, 1), "Quarter close"),
                    new TrendPoint("Jan", alignment, "Current quarter start"),
                    new TrendPoint("Feb", Math.Clamp(alignment + 0.01, 0, 1), "Recent check-in"),
                    new TrendPoint("Mar", alignment, "Latest snapshot")
                ]
            }
        };
    }
}

public sealed class UbiState
{
    public CompanyProfile Company { get; set; } = new();
    public List<EmployeeProfile> Employees { get; set; } = [];
    public List<InterventionRecord> Interventions { get; set; } = [];
    public List<ActivityRecord> Activity { get; set; } = [];
}

public sealed record CompanyProfile
{
    public string CompanyId { get; init; } = string.Empty;
    public string LegalName { get; init; } = string.Empty;
    public string Tagline { get; init; } = string.Empty;
    public string PurposeStatement { get; init; } = string.Empty;
}

public sealed class EmployeeProfile
{
    public string EmployeeId { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string RoleTitle { get; set; } = string.Empty;
    public string EmploymentStatus { get; set; } = "ACTIVE";
    public DateOnly HireDate { get; set; }
    public decimal UbiAnnualGrant { get; set; }
    public decimal AssetContributionAnnual { get; set; }
    public string VestingSchedule { get; set; } = "CLIFF_2_YEARS";
    public DateOnly VestingStartDate { get; set; }
    public EstateProfile Estate { get; set; } = new();
    public PresenceProfile Presence { get; set; } = new();
}

public sealed class EstateProfile
{
    public decimal CurrentBalance { get; set; }
    public decimal MonthlyChange { get; set; }
    public decimal PassiveIncomeMonthly { get; set; }
    public decimal CurrentOutrightOwnership { get; set; }
    public int CliffMonthsRemaining { get; set; }
    public int VestingPercent { get; set; }
    public List<AssetAllocationItem> AssetComposition { get; set; } = [];
    public List<ProjectionPoint> Projections { get; set; } = [];
    public List<LedgerEntry> Transactions { get; set; } = [];
}

public sealed class PresenceProfile
{
    public double AlignmentScore { get; set; }
    public double ProjectCompletionRate { get; set; }
    public double SelfReportedAlignment { get; set; }
    public double PeerFeedbackAverage { get; set; }
    public double AutonomyExperience { get; set; }
    public string Trend { get; set; } = "Stable";
    public List<TrendPoint> TrendHistory { get; set; } = [];
}

public sealed class InterventionRecord
{
    public string InterventionId { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Severity { get; set; } = "YELLOW";
    public string Status { get; set; } = "OPEN";
    public string Summary { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}

public sealed class ActivityRecord
{
    public DateTime OccurredAtUtc { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tone { get; set; } = "blue";
}

public sealed class NewEmployeeRequest
{
    [Required]
    public string LegalName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = "Engineering";

    [Required]
    public string RoleTitle { get; set; } = string.Empty;

    [Required]
    public DateTime HireDate { get; set; } = DateTime.Today;

    [Range(typeof(decimal), "1", "1000000000")]
    public decimal UbiAnnualGrant { get; set; } = 60_000m;

    [Range(typeof(decimal), "0", "1000000000")]
    public decimal AssetContributionAnnual { get; set; } = 50_000m;
}

public sealed class NewInterventionRequest
{
    [Required]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    public string Owner { get; set; } = "Leadership Team";

    [Required]
    public string Severity { get; set; } = "YELLOW";

    [Required]
    public string Summary { get; set; } = string.Empty;

    [Required]
    public string Recommendation { get; set; } = string.Empty;
}

public sealed class UpdateCompanyPurposeRequest
{
    [Required]
    public string PurposeStatement { get; set; } = string.Empty;
}

public sealed record UbiWorkspace(
    string CompanyName,
    string Tagline,
    string PurposeStatement,
    int ActiveEmployees,
    int OpenInterventions,
    decimal MonthlyFlow,
    string Readiness,
    IReadOnlyList<Pillar> ExperiencePillars,
    IReadOnlyList<WorkflowStage> WorkflowStages,
    IReadOnlyList<Guardrail> Guardrails,
    IReadOnlyList<ActivityItem> RecentActivity,
    EmployeeEstateDashboard Estate,
    EmployeePresenceDashboard Presence,
    LeadershipDashboard Leadership);

public sealed record Pillar(string Title, string Description, string Tone);
public sealed record WorkflowStage(string Title, string Description, string Status);
public sealed record Guardrail(string Title, string Description);
public sealed record ActivityItem(string DateLabel, string Title, string Description, string Tone);

public sealed record EmployeeEstateDashboard(
    string EmployeeName,
    decimal CurrentBalance,
    decimal MonthlyChange,
    decimal PassiveIncomeMonthly,
    decimal CurrentOutrightOwnership,
    int CliffMonthsRemaining,
    int VestingPercent,
    IReadOnlyList<AssetAllocationItem> AssetComposition,
    IReadOnlyList<ProjectionPoint> Projections,
    IReadOnlyList<LedgerEntry> Transactions);

public sealed record AssetAllocationItem(string Label, decimal Amount, int Percent, string Tone);
public sealed record ProjectionPoint(string Label, decimal Amount, string Note);
public sealed record LedgerEntry(string DateLabel, string Description, decimal Amount, string Tone);

public sealed record EmployeePresenceDashboard(
    string EmployeeName,
    double AlignmentScore,
    string StatusLabel,
    string StatusTone,
    int ReviewCadenceDays,
    string Summary,
    IReadOnlyList<ScoreBreakdownItem> Breakdown,
    IReadOnlyList<TrendPoint> Trend,
    IReadOnlyList<FrictionPoint> FrictionPoints);

public sealed record ScoreBreakdownItem(string Label, double Score, string Note);
public sealed record TrendPoint(string Label, double Score, string Note);
public sealed record FrictionPoint(string Title, string Description, string Tone);

public sealed record LeadershipDashboard(
    double OverallHealth,
    string HealthLabel,
    int GreenCount,
    int YellowCount,
    int RedCount,
    double AvgTenureYears,
    double RecommendationRate,
    IReadOnlyList<DepartmentScore> DepartmentScores,
    IReadOnlyList<FlaggedEmployee> Flags,
    IReadOnlyList<HealthMetric> HealthMetrics,
    IReadOnlyList<PlaybookAction> InterventionPlaybook);

public sealed record DepartmentScore(string Department, double Score, int Employees);
public sealed record FlaggedEmployee(string Name, string Department, double Score, string Trend, string Notes, string Recommendation, string Tone);
public sealed record HealthMetric(string Label, string Value, string Note);
public sealed record PlaybookAction(string Title, string Description);

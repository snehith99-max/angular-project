import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnAssignvisitsummaryassignedComponent } from './crm-trn-assignvisitsummaryassigned.component';

describe('CrmTrnAssignvisitsummaryassignedComponent', () => {
  let component: CrmTrnAssignvisitsummaryassignedComponent;
  let fixture: ComponentFixture<CrmTrnAssignvisitsummaryassignedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnAssignvisitsummaryassignedComponent]
    });
    fixture = TestBed.createComponent(CrmTrnAssignvisitsummaryassignedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

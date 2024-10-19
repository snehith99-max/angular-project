import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnAssignvisitsummaryupcomingComponent } from './crm-trn-assignvisitsummaryupcoming.component';

describe('CrmTrnAssignvisitsummaryupcomingComponent', () => {
  let component: CrmTrnAssignvisitsummaryupcomingComponent;
  let fixture: ComponentFixture<CrmTrnAssignvisitsummaryupcomingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnAssignvisitsummaryupcomingComponent]
    });
    fixture = TestBed.createComponent(CrmTrnAssignvisitsummaryupcomingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

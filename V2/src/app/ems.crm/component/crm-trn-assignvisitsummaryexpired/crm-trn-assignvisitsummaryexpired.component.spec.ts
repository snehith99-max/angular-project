import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnAssignvisitsummaryexpiredComponent } from './crm-trn-assignvisitsummaryexpired.component';

describe('CrmTrnAssignvisitsummaryexpiredComponent', () => {
  let component: CrmTrnAssignvisitsummaryexpiredComponent;
  let fixture: ComponentFixture<CrmTrnAssignvisitsummaryexpiredComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnAssignvisitsummaryexpiredComponent]
    });
    fixture = TestBed.createComponent(CrmTrnAssignvisitsummaryexpiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

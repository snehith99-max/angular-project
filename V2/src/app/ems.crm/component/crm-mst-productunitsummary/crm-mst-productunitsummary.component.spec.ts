import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstProductunitsummaryComponent } from './crm-mst-productunitsummary.component';

describe('CrmMstProductunitsummaryComponent', () => {
  let component: CrmMstProductunitsummaryComponent;
  let fixture: ComponentFixture<CrmMstProductunitsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstProductunitsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmMstProductunitsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

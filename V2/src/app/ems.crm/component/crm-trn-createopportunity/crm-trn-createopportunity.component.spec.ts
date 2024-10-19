import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnCreateopportunityComponent } from './crm-trn-createopportunity.component';

describe('CrmTrnCreateopportunityComponent', () => {
  let component: CrmTrnCreateopportunityComponent;
  let fixture: ComponentFixture<CrmTrnCreateopportunityComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnCreateopportunityComponent]
    });
    fixture = TestBed.createComponent(CrmTrnCreateopportunityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

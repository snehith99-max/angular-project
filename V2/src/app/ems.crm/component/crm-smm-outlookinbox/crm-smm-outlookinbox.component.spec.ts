import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookinboxComponent } from './crm-smm-outlookinbox.component';

describe('CrmSmmOutlookinboxComponent', () => {
  let component: CrmSmmOutlookinboxComponent;
  let fixture: ComponentFixture<CrmSmmOutlookinboxComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookinboxComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookinboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

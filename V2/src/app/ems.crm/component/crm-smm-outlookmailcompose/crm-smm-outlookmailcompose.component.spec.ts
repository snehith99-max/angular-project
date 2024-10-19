import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookmailcomposeComponent } from './crm-smm-outlookmailcompose.component';

describe('CrmSmmOutlookmailcomposeComponent', () => {
  let component: CrmSmmOutlookmailcomposeComponent;
  let fixture: ComponentFixture<CrmSmmOutlookmailcomposeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookmailcomposeComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookmailcomposeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

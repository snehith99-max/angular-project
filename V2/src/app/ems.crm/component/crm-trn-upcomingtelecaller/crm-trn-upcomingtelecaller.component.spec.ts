import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnUpcomingtelecallerComponent } from './crm-trn-upcomingtelecaller.component';

describe('CrmTrnUpcomingtelecallerComponent', () => {
  let component: CrmTrnUpcomingtelecallerComponent;
  let fixture: ComponentFixture<CrmTrnUpcomingtelecallerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnUpcomingtelecallerComponent]
    });
    fixture = TestBed.createComponent(CrmTrnUpcomingtelecallerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
